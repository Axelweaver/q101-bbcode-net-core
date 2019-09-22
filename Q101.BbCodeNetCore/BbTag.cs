using System;
using Q101.BbCodeNetCore.Enums;

namespace Q101.BbCodeNetCore
{
    public class BbTag
    {
        public const string ContentPlaceholderName = "content";

        public BbTag(string name, 
                    string openTagTemplate, 
                    string closeTagTemplate, 
                    bool autoRenderContent, 
                    BbTagClosingStyle tagClosingClosingStyle, 
                    Func<string, string> contentTransformer, 
                    bool enableIterationElementBehavior, 
                    params BbAttribute[] attributes)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (openTagTemplate == null)
            {
                throw new ArgumentNullException(nameof(openTagTemplate));
            }

            if (closeTagTemplate == null)
            {
                throw new ArgumentNullException(nameof(closeTagTemplate));
            }

            if (!Enum.IsDefined(typeof(BbTagClosingStyle), tagClosingClosingStyle))
            {
                throw new ArgumentException(nameof(tagClosingClosingStyle));
            }

            Name = name;

            OpenTagTemplate = openTagTemplate;

            CloseTagTemplate = closeTagTemplate;

            AutoRenderContent = autoRenderContent;

            TagClosingStyle = tagClosingClosingStyle;

            ContentTransformer = contentTransformer;

            EnableIterationElementBehavior = enableIterationElementBehavior;

            Attributes = attributes ?? new BbAttribute[0];
        }
        
        public BbTag(string name, 
                     string openTagTemplate, 
                     string closeTagTemplate, 
                     bool autoRenderContent, 
                     BbTagClosingStyle tagClosingClosingStyle, 
                     Func<string, string> contentTransformer, 
                     params BbAttribute[] attributes)
            : this(name, 
                   openTagTemplate, 
                   closeTagTemplate, 
                   autoRenderContent, 
                   tagClosingClosingStyle, 
                   contentTransformer, 
                   false, 
                   attributes) { }

        public BbTag(string name, 
                    string openTagTemplate, 
                    string closeTagTemplate, 
                    bool autoRenderContent, 
                    bool requireClosingTag, 
                    Func<string, string> contentTransformer, 
                    params BbAttribute[] attributes)
            : this(name, 
                    openTagTemplate, 
                    closeTagTemplate, 
                    autoRenderContent, 
                    requireClosingTag 
                        ? BbTagClosingStyle.RequiresClosingTag 
                        : BbTagClosingStyle.AutoCloseElement, 
                    contentTransformer, attributes) { }

        public BbTag(string name, 
                     string openTagTemplate, 
                     string closeTagTemplate, 
                     bool autoRenderContent, 
                     bool requireClosingTag, 
                     params BbAttribute[] attributes)
            : this(name, 
                    openTagTemplate, 
                    closeTagTemplate, 
                    autoRenderContent, 
                    requireClosingTag, 
                    null, 
                    attributes) { }

        public BbTag(string name, 
                     string openTagTemplate, 
                     string closeTagTemplate, 
                     params BbAttribute[] attributes)
            : this(name, 
                    openTagTemplate, 
                    closeTagTemplate, 
                    true, 
                    true, 
                    attributes) { }

        public string Name { get; }

        public string OpenTagTemplate { get; }

        public string CloseTagTemplate { get; }

        public bool AutoRenderContent { get; }

        public bool EnableIterationElementBehavior { get; }

        public bool RequiresClosingTag => TagClosingStyle == BbTagClosingStyle.RequiresClosingTag;

        public BbTagClosingStyle TagClosingStyle { get; }

        /// <summary>
        /// allows for custom modification of the tag content before rendering takes place
        /// </summary>
        public Func<string, string> ContentTransformer { get; }

        public BbAttribute[] Attributes { get; }

        public BbAttribute FindAttribute(string name)
        {
            return Array.Find(Attributes, 
                a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}