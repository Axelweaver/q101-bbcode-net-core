namespace Q101.BbCodeNetCore.Abstract
{
    public interface IAttributeRenderingContext
    {
        BbAttribute Attribute { get; }

        string AttributeValue { get; }

        string GetAttributeValueById(string id);
    }
}
