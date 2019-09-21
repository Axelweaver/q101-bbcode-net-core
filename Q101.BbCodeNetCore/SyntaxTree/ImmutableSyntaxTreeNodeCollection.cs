using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Q101.BbCodeNetCore.Abstract;

namespace Q101.BbCodeNetCore.SyntaxTree
{
    public class ImmutableSyntaxTreeNodeCollection : ReadOnlyCollection<SyntaxTreeNode>, ISyntaxTreeNodeCollection
    {
        public ImmutableSyntaxTreeNodeCollection(IEnumerable<SyntaxTreeNode> list) : base(list.ToArray())
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
        }

        internal ImmutableSyntaxTreeNodeCollection(IList<SyntaxTreeNode> list, bool isFresh)
            : base(isFresh ? list : list.ToArray()) { }

        static readonly ImmutableSyntaxTreeNodeCollection empty 
            = new ImmutableSyntaxTreeNodeCollection(new SyntaxTreeNode[0], true);

        public static ImmutableSyntaxTreeNodeCollection Empty => empty;
    }
}
