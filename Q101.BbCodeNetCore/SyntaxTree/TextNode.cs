using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Q101.BbCodeNetCore.SyntaxTree
{
    public sealed class TextNode : SyntaxTreeNode
    {
        public TextNode(string text) : this(text, null) { }

        public TextNode(string text, string htmlTemplate) : base(null)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Text = text;

            HtmlTemplate = htmlTemplate;
        }


        public string Text { get; }

        public string HtmlTemplate { get; }

        public override string ToHtml()
        {
            return HtmlTemplate == null 
                ? HttpUtility.HtmlEncode(Text) 
                : HtmlTemplate.Replace("${content}", 
                                        HttpUtility.HtmlEncode(Text));
        }

        public override string ToBbCode()
        {
            var result = 
                Text.Replace("\\", "\\\\")
                    .Replace("[", "\\[")
                    .Replace("]", "\\]");

            return result;
        }

        public override string ToText()
        {
            return Text;
        }

        public override SyntaxTreeNode SetSubNodes(IEnumerable<SyntaxTreeNode> subNodes)
        {
            if (subNodes == null)
            {
                throw new ArgumentNullException(nameof(subNodes));
            }

            if (subNodes.Any())
            {
                throw new ArgumentException("subNodes cannot contain any nodes for a TextNode");
            }

            return this;
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
            var casted = (TextNode)b;

            return Text == casted.Text && HtmlTemplate == casted.HtmlTemplate;
        }
    }
}