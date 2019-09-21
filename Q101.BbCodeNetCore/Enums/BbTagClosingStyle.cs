namespace Q101.BbCodeNetCore.Enums
{
    public enum BbTagClosingStyle
    {
        RequiresClosingTag = 0,

        AutoCloseElement = 1,

        // leaf elements have no content - they are closed immediately
        LeafElementWithoutContent = 2, 
    }
}
