using Q101.BbCodeNetCore.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Q101.BbCodeNetCore.Comparers;
using Q101.BbCodeNetCore.Enums;

namespace Q101.BbCodeNetCore
{
    public static class BbCode
    {
        static readonly BbCodeParser DefaultParser = GetParser();


        public static string ToHtml(string bbCode)
        {
            if (bbCode == null)
            {
                throw new ArgumentNullException(nameof(bbCode));
            }

            return DefaultParser.ToHtml(bbCode);
        }

        static BbCodeParser GetParser()
        {
            return new BbCodeParser(ErrorMode.ErrorFree, null, new[]
                {
                    new BbTag("b", "<b>", "</b>"), 
                    new BbTag("i", "<span style=\"font-style:italic;\">", "</span>"), 
                    new BbTag("u", "<span style=\"text-decoration:underline;\">", "</span>"), 
                    new BbTag("code", "<pre class=\"prettyprint\">", "</pre>"), 
                    new BbTag("img", "<img src=\"${content}\" />", "", false, true), 
                    new BbTag("quote", "<blockquote>", "</blockquote>"), 
                    new BbTag("list", "<ul>", "</ul>"), 
                    new BbTag("*", "<li>", "</li>", true, false), 
                    new BbTag("url", 
                        "<a href=\"${href}\">", 
                        "</a>", 
                        new BbAttribute("href", ""), 
                        new BbAttribute("href", "href")), 
                });
        }

        public static readonly string InvalidBBCodeTextChars = @"[]\";

        /// <summary>
        /// Encodes an arbitrary string to be valid BBCode. Example: "[b]" => "\[b\]". The resulting string is safe against
        /// BBCode-Injection attacks.
        /// </summary>
        public static string EscapeText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            int escapeCount = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '[' || text[i] == ']' || text[i] == '\\')
                {
                    escapeCount++;
                }
            }

            if (escapeCount == 0) return text;

            var output = new char[text.Length + escapeCount];

            int outputWritePos = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '[' || text[i] == ']' || text[i] == '\\')
                {
                    output[outputWritePos++] = '\\';
                }

                output[outputWritePos++] = text[i];
            }

            return new string(output);
        }

        /// <summary>
        /// Decodes a string of BBCode that only contains text (no tags). Example: "\[b\]" => "[b]". This is the reverse
        /// oepration of EscapeText.
        /// </summary>
        public static string UnescapeText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var result = text.Replace("\\[", "[")
                             .Replace("\\]", "]")
                             .Replace("\\\\", "\\");

            return result;
        }

        public static SyntaxTreeNode ReplaceTextSpans(SyntaxTreeNode node, 
                                                      Func<string, IList<TextSpanReplaceInfo>> getTextSpansToReplace, 
                                                      Func<TagNode, bool> tagFilter)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (getTextSpansToReplace == null)
            {
                throw new ArgumentNullException(nameof(getTextSpansToReplace));
            }

            if (node is TextNode)
            {
                var text = ((TextNode)node).Text;

                var replacements = getTextSpansToReplace(text);

                if (replacements == null || replacements.Count == 0)
                {
                    return node;
                }

                var replacementNodes = new List<SyntaxTreeNode>(replacements.Count * 2 + 1);

                var lastPos = 0;

                foreach (var r in replacements)
                {
                    if (r.Index < lastPos)
                    {
                        throw new ArgumentException(
                            "the replacement text spans must be ordered by index and non-overlapping");
                    }

                    if (r.Index > text.Length - r.Length)
                    {
                        throw new ArgumentException(
                            "every replacement text span must reference a range within the text node");
                    }

                    if (r.Index != lastPos)
                    {
                        replacementNodes.Add(new TextNode(text.Substring(lastPos, r.Index - lastPos)));
                    }

                    if (r.Replacement != null)
                    {
                        replacementNodes.Add(r.Replacement);
                    }

                    lastPos = r.Index + r.Length;
                }

                if (lastPos != text.Length)
                {
                    replacementNodes.Add(new TextNode(text.Substring(lastPos)));
                }

                return new SequenceNode(replacementNodes);
            }
            else
            {
                var fixedSubNodes = node.SubNodes.Select(n =>
                {
                    if (n is TagNode && (tagFilter != null && !tagFilter((TagNode)n))) return n; //skip filtered tags

                    var repl = ReplaceTextSpans(n, getTextSpansToReplace, tagFilter);

                    Debug.Assert(repl != null);

                    return repl;

                }).ToList();

                if (fixedSubNodes.SequenceEqual(node.SubNodes, ReferenceEqualityComparer<SyntaxTreeNode>.Instance))
                {
                    return node;
                }

                return node.SetSubNodes(fixedSubNodes);
            }
        }

        public static void VisitTextNodes(SyntaxTreeNode node, Action<string> visitText, Func<TagNode, bool> tagFilter)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (visitText == null)
            {
                throw new ArgumentNullException(nameof(visitText));
            }

            if (node is TextNode)
            {
                visitText(((TextNode)node).Text);
            }
            else
            {
                if (node is TagNode && (tagFilter != null && !tagFilter((TagNode)node)))
                {
                    return; //skip filtered tags
                }

                foreach (var subNode in node.SubNodes)
                {
                    VisitTextNodes(subNode, visitText, tagFilter);
                }
            }
        }
    }
}
