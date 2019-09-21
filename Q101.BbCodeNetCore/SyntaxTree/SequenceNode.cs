using System;
using System.Collections.Generic;
using System.Linq;

namespace Q101.BbCodeNetCore.SyntaxTree
{
    public sealed class SequenceNode : SyntaxTreeNode
    {
        public SequenceNode() { }

        public SequenceNode(SyntaxTreeNodeCollection subNodes) : base(subNodes)
        {
            if (subNodes == null)
            {
                throw new ArgumentNullException(nameof(subNodes));
            }
        }

        public SequenceNode(IEnumerable<SyntaxTreeNode> subNodes) : base(subNodes)
        {
            if (subNodes == null)
            {
                throw new ArgumentNullException(nameof(subNodes));
            }
        }

        public override string ToHtml()
        {
            var result = 
                string.Concat(
                    SubNodes
                        .Select(s => s.ToHtml())
                        .ToArray());

            return result;
        }

        public override string ToBbCode()
        {
            var result = 
                string.Concat(
                    SubNodes
                        .Select(s => s.ToBbCode())
                        .ToArray());

            return result;
        }

        public override string ToText()
        {
            var result = 
                string.Concat(
                    SubNodes
                        .Select(s => s.ToText())
                        .ToArray());

            return result;
        }

        public override SyntaxTreeNode SetSubNodes(IEnumerable<SyntaxTreeNode> subNodes)
        {
            if (subNodes == null)
            {
                throw new ArgumentNullException(nameof(subNodes));
            }

            var sequenceNode = new SequenceNode(subNodes);

            return sequenceNode;
        }

        internal override SyntaxTreeNode AcceptVisitor(SyntaxTreeVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this);
        }

        protected override bool EqualsCore(SyntaxTreeNode b) => true;
    }
}
