using System;
using System.Collections.Generic;
using Q101.BbCodeNetCore.Abstract;

namespace Q101.BbCodeNetCore.SyntaxTree
{
    public abstract class SyntaxTreeNode : IEquatable<SyntaxTreeNode>
    {
        protected SyntaxTreeNode()
        {
            SubNodes = new SyntaxTreeNodeCollection();
        }

        protected SyntaxTreeNode(ISyntaxTreeNodeCollection subNodes)
        {
            SubNodes = subNodes ?? new SyntaxTreeNodeCollection();
        }

        protected SyntaxTreeNode(IEnumerable<SyntaxTreeNode> subNodes)
        {
            SubNodes = subNodes == null 
                ? new SyntaxTreeNodeCollection() 
                : new SyntaxTreeNodeCollection(subNodes);
        }

        public override string ToString()
        {
            return ToBbCode();
        }

        //not null
        public ISyntaxTreeNodeCollection SubNodes { get; private set; }

        public abstract string ToHtml();

        public abstract string ToBbCode();

        public abstract string ToText();

        public abstract SyntaxTreeNode SetSubNodes(IEnumerable<SyntaxTreeNode> subNodes);

        internal abstract SyntaxTreeNode AcceptVisitor(SyntaxTreeVisitor visitor);

        protected abstract bool EqualsCore(SyntaxTreeNode b);

        //equality members
        public bool Equals(SyntaxTreeNode other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SyntaxTreeNode);
        }

        public override int GetHashCode()
        {
            // TODO
            return base.GetHashCode(); 
        }

        public static bool operator ==(SyntaxTreeNode a, SyntaxTreeNode b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (ReferenceEquals(a, null))
            {
                return false;
            }

            if (ReferenceEquals(b, null))
            {
                return false;
            }

            if (a.GetType() != b.GetType())
            {
                return false;
            }

            if (a.SubNodes.Count != b.SubNodes.Count)
            {
                return false;
            }

            if (!ReferenceEquals(a.SubNodes, b.SubNodes))
            {
                for (int i = 0; i < a.SubNodes.Count; i++)
                {
                    if (a.SubNodes[i] != b.SubNodes[i])
                    {
                        return false;
                    }
                }
            }

            return a.EqualsCore(b);
        }

        public static bool operator !=(SyntaxTreeNode a, SyntaxTreeNode b)
        {
            return !(a == b);
        }
    }
}