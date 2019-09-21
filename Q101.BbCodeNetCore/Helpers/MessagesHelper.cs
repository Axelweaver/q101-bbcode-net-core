using System.Resources;

namespace Q101.BbCodeNetCore.Helpers
{
    static class MessagesHelper
    {
        static readonly ResourceManager ResMgr;

        static MessagesHelper()
        {
            ResMgr = new ResourceManager(typeof(Messages));
        }

        public static string GetString(string key)
        {
            return ResMgr.GetString(key);
        }

        public static string GetString(string key, params string[] parameters)
        {
            return string.Format(ResMgr.GetString(key), parameters);
        }
    }
}
