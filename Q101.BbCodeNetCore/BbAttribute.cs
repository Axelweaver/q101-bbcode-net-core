using System;
using Q101.BbCodeNetCore.Abstract;
using Q101.BbCodeNetCore.Enums;

namespace Q101.BbCodeNetCore
{
    public class BbAttribute
    {
        public BbAttribute(string id, string name)
            : this(id, name, null, HtmlEncodingMode.HtmlAttributeEncode) { }

        public BbAttribute(string id, string name, Func<IAttributeRenderingContext, string> contentTransformer)
            : this(id, name, contentTransformer, HtmlEncodingMode.HtmlAttributeEncode) { }

        public BbAttribute(string id, 
                           string name, 
                           Func<IAttributeRenderingContext, string> contentTransformer, 
                           HtmlEncodingMode htmlEncodingMode)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!Enum.IsDefined(typeof(HtmlEncodingMode), htmlEncodingMode))
            {
                throw new ArgumentException(nameof(htmlEncodingMode));
            }

            Id = id;

            Name = name;

            ContentTransformer = contentTransformer;

            HtmlEncodingMode = htmlEncodingMode;
        }

        /// <summary>
        /// ID is used to reference the attribute value
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Name is used during parsing
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// allows for custom modification of the attribute value before rendering takes place
        /// </summary>
        public Func<IAttributeRenderingContext, string> ContentTransformer { get; }

        public HtmlEncodingMode HtmlEncodingMode { get; set; }

        public static Func<IAttributeRenderingContext, string> AdaptLegacyContentTransformer(Func<string, string> contentTransformer)
        {
            return contentTransformer == null 
                ? (Func<IAttributeRenderingContext, string>)null 
                : ctx => contentTransformer(ctx.AttributeValue);
        }
    }
}
