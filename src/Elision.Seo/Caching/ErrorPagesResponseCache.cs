using System;
using Sitecore;
using Sitecore.Configuration;

namespace Elision.Seo.Caching
{
    public class ErrorPagesResponseCache : Sitecore.Caching.CustomCache
    {
        public ErrorPagesResponseCache(long maxSize) : base("Elision.ErrorPagesResponseCache", maxSize)
        {
        }

        public static readonly ErrorPagesResponseCache Current = new ErrorPagesResponseCache(StringUtil.ParseSizeString(Settings.GetSetting("Elision.ErrorPagesResponseCache", "50MB")));

        public string Get(string cacheKey)
        {
            return GetString(cacheKey);
        }

        public void Set(string cacheKey, string value)
        {
            SetString(cacheKey, value, DateTime.Now.AddDays(1));
        }

        public void Remove(string cacheKey)
        {
            base.Remove(cacheKey);
        }
    }
}
