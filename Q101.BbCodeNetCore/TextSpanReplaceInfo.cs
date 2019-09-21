using System;
using Q101.BbCodeNetCore.SyntaxTree;

namespace Q101.BbCodeNetCore
{
    public class TextSpanReplaceInfo
    {
        public TextSpanReplaceInfo(int index, int length, SyntaxTreeNode replacement)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            Index = index;

            Length = length;

            Replacement = replacement;
        }

        public int Index { get; }

        public int Length { get; }

        public SyntaxTreeNode Replacement { get; }
    }
}
