using Q101.BbCodeNetCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Q101.BbCodeNetCore.SyntaxTree
{
    public sealed class TagNode : SyntaxTreeNode
    {
        private string CoverByTemplate(string value) => $"${{{value}}}";

        public TagNode(BbTag tag) : this(tag, null) { }

        public TagNode(BbTag tag, IEnumerable<SyntaxTreeNode> subNodes) : base(subNodes)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            Tag = tag;

            AttributeValues = new Dictionary<BbAttribute, string>();
        }

        public BbTag Tag { get; }

        public IDictionary<BbAttribute, string> AttributeValues { get; private set; }

        public override string ToHtml()
        {
            var content = GetContent();

            var openTabTemplate = ReplaceAttributeValues(Tag.OpenTagTemplate, content);

            var autoRenderContent = Tag.AutoRenderContent ? content : null;

            var closeTagTemplate = ReplaceAttributeValues(Tag.CloseTagTemplate, content);

            var result = $"{openTabTemplate}{autoRenderContent}{closeTagTemplate}";

            return result;
        }

        public override string ToBbCode()
        {
            var content = string.Concat(SubNodes.Select(s => s.ToBbCode()).ToArray());

            var defAttr = Tag.FindAttribute(string.Empty);

            var attrStringBuilder = new StringBuilder();

            if (defAttr != null)
            {
                if (AttributeValues.ContainsKey(defAttr))
                {
                    var attrString = $"={AttributeValues[defAttr]}";

                    attrStringBuilder.Append(attrString);
                }
                    
            }
            foreach (var attrKvp in AttributeValues)
            {
                if (string.IsNullOrEmpty(attrKvp.Key.Name))
                {
                    continue;
                }

                var attrString = $" {attrKvp.Key.Name}={attrKvp.Value}";

                attrStringBuilder.Append(attrString);
            }

            var attrs = attrStringBuilder.ToString();

            var result = $"[{Tag.Name}{attrs}]{content}[/{Tag.Name}]";

            return result;
        }
        public override string ToText()
        {
            return string.Concat(SubNodes.Select(s => s.ToText()).ToArray());
        }

        string GetContent()
        {
            var content = string.Concat(SubNodes.Select(s => s.ToHtml()).ToArray());

            return Tag.ContentTransformer == null 
                ? content 
                : Tag.ContentTransformer(content);
        }
        string ReplaceAttributeValues(string template, string content)
        {
            var attributesWithValues = (from attr in Tag.Attributes
                                        group attr by attr.Id into gAttrById
                                        let val = (from attr in gAttrById
                                                   let val = TryGetValue(attr)
                                                   where val != null
                                                   select new { attr, val }).FirstOrDefault()
                                        select new { attrID = gAttrById.Key, attrAndVal = val }).ToList();

            var attrValuesById = attributesWithValues
                .Where(x => x.attrAndVal != null)
                .ToDictionary(x => x.attrID, x => x.attrAndVal.val);

            if (!attrValuesById.ContainsKey(BbTag.ContentPlaceholderName))
            {
                attrValuesById.Add(BbTag.ContentPlaceholderName, content);
            }

            var output = template;

            foreach (var x in attributesWithValues)
            {
                var placeholderStr = CoverByTemplate(x.attrID);// "${" + x.attrID + "}";

                if (x.attrAndVal != null)
                {
                    //replace attributes with values
                    var rawValue = x.attrAndVal.val;

                    var attribute = x.attrAndVal.attr;

                    output = 
                        ReplaceAttribute(output, 
                                         attribute, 
                                         rawValue, 
                                         placeholderStr, 
                                         attrValuesById);
                }
            }

            //replace empty attributes
            var attributeIDsWithValues = 
                new HashSet<string>(attributesWithValues
                    .Where(x => x.attrAndVal != null)
                    .Select(x => x.attrID));

            var emptyAttributes = 
                Tag.Attributes.Where(attr => !attributeIDsWithValues.Contains(attr.Id))
                              .ToList();
            
            foreach (var attr in emptyAttributes)
            {
                var placeholderStr = CoverByTemplate(attr.Id);

                output = ReplaceAttribute(output, attr, null, placeholderStr, attrValuesById);
            }

            var oldValue = CoverByTemplate(BbTag.ContentPlaceholderName);

            output = output.Replace(oldValue, content);

            return output;
        }

        static string ReplaceAttribute(string output, BbAttribute attribute, string rawValue, string placeholderStr, Dictionary<string, string> attrValuesById)
        {
            string effectiveValue;
            if (attribute.ContentTransformer == null)
            {
                effectiveValue = rawValue;
            }
            else
            {
                var ctx = new AttributeRenderingContextImpl(attribute, rawValue, attrValuesById);

                effectiveValue = attribute.ContentTransformer(ctx);
            }

            if (effectiveValue == null)
            {
                effectiveValue = string.Empty;
            }

            var encodedValue =
                attribute.HtmlEncodingMode == HtmlEncodingMode.HtmlAttributeEncode 
                    ? HttpUtility.HtmlAttributeEncode(effectiveValue)
                    : attribute.HtmlEncodingMode == HtmlEncodingMode.HtmlEncode 
                          ? HttpUtility.HtmlEncode(effectiveValue)
                          : effectiveValue;

            output = output.Replace(placeholderStr, encodedValue);

            return output;
        }

        string TryGetValue(BbAttribute attr)
        {
            string value;

            AttributeValues.TryGetValue(attr, out value);

            return value;
        }

        public override SyntaxTreeNode SetSubNodes(IEnumerable<SyntaxTreeNode> subNodes)
        {
            if (subNodes == null)
            {
                throw new ArgumentNullException(nameof(subNodes));
            }

            var tagNode = 
                new TagNode(Tag, subNodes)
                {
                    AttributeValues = new Dictionary<BbAttribute, string>(AttributeValues),
                };

            return tagNode;
        }

        internal override SyntaxTreeNode AcceptVisitor(SyntaxTreeVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this);
        }

        protected override bool EqualsCore(SyntaxTreeNode b)
        {
            var casted = (TagNode)b;

            var equals = 
                Tag == casted.Tag 
                    && AttributeValues.All(attr => casted.AttributeValues[attr.Key] == attr.Value) 
                    && casted.AttributeValues.All(attr => AttributeValues[attr.Key] == attr.Value);

            return equals;
        }
    }
}
