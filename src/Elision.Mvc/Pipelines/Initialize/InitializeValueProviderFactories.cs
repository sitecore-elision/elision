using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;

namespace Elision.Mvc.Pipelines.Initialize
{
    public class InitializeValueProviderFactories
    {
        public virtual void Process(PipelineArgs args)
        {
            foreach (var valueProviderFactoryType in Assembly
                .GetAssembly(GetType())
                .GetExportedTypes()
                .Where(t => !t.IsAbstract && typeof (ValueProviderFactory).IsAssignableFrom(t)))
            {
                try
                {
                    Log.Info("Registering ValueProvider " + valueProviderFactoryType.AssemblyQualifiedName, this);
                    var instance = Elision.Ioc.OnDemandResolver.Current.Resolve(valueProviderFactoryType) as ValueProviderFactory;
                    if (instance != null)
                        ValueProviderFactories.Factories.Add(instance);
                    else
                        Log.Warn("Unable to create instance of ValueProviderFactory for type " + valueProviderFactoryType.AssemblyQualifiedName, this);
                }
                catch (Exception ex)
                {
                    Log.Error(
                        "Unable to register value provider factory for type " +
                        valueProviderFactoryType.AssemblyQualifiedName, ex, this);
               }
            }
        }
    }
}
