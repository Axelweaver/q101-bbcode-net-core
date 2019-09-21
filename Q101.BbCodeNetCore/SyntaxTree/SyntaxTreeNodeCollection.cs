using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Q101.BbCodeNetCore.Abstract;

namespace Q101.BbCodeNetCore.SyntaxTree
{
    public class SyntaxTreeNodeCollection : Collection<SyntaxTreeNode>, ISyntaxTreeNodeCollection
    {
        public SyntaxTreeNodeCollection() : base() { }

        public SyntaxTreeNodeCollection(IEnumerable<SyntaxTreeNode> list) : base(list.ToArray())
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
        }

        protected override void SetItem(int index, SyntaxTreeNode item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, SyntaxTreeNode item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            base.InsertItem(index, item);
        }
    }
}
