using System.Collections.Generic;
using Q101.BbCodeNetCore.Abstract;

namespace Q101.BbCodeNetCore.SyntaxTree
{
    class AttributeRenderingContextImpl : IAttributeRenderingContext
    {
        public AttributeRenderingContextImpl(BbAttribute attribute, 
                                             string attributeValue, 
                                             IDictionary<string, string> getAttributeValueByIdData)
        {
            Attribute = attribute;

            AttributeValue = attributeValue;

            GetAttributeValueByIdData = getAttributeValueByIdData;
        }

        public BbAttribute Attribute { get; }

        public string AttributeValue { get; }

        public IDictionary<string, string> GetAttributeValueByIdData { get; }

        public string GetAttributeValueById(string id)
        {
            string value;

            if (!GetAttributeValueByIdData.TryGetValue(id, out value))
            {
                return null;
            }

            return value;
        }
    }
}
