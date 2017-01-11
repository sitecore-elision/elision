using System;
using Sitecore.Mvc.Helpers;
using Sitecore.Reflection;

namespace Elision.Foundation.Ioc
{
    public class ElisionObjectFactory : IFactory
    {
        public object GetObject(string identifier)
        {
            if (!TypeHelper.LooksLikeTypeName(identifier))
                return null;

            var type = Type.GetType(identifier);
            return GetObject(type);
        }

        public object GetObject(Type type)
        {
            return type == null ? null : OnDemandResolver.Current.Resolve(type);
        }
    }
}
