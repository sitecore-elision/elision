using Elision.Foundation.Kernel;
using Elision.Foundation.Rules;
using Elision.Foundation.Themes;
using Elision.Foundation.UpdateReferences;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Elision.Foundation.Ioc
{
    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRulesRunner, RulesRunner>();
            serviceCollection.AddTransient<IThemeRetriever, ThemeRetriever>();
            serviceCollection.AddTransient<ITreeReferenceUpdater, TreeReferenceUpdater>(); 
            serviceCollection.AddMvcControllers("Elision.Foundation.*");
        }
    }
}
