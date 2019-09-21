using System;

namespace Q101.BbCodeNetCore
{
    [Serializable]
    public class BbCodeParsingException : Exception
    {
        public BbCodeParsingException() { }

        public BbCodeParsingException(string message) : base(message) { }
    }
}