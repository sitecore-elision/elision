using System;

namespace Elision.Foundation.Kernel.Diagnostics
{
    public class ProfileOperation : IDisposable
    {
        public ProfileOperation(string name)
        {
            Sitecore.Diagnostics.Profiler.StartOperation(name);
        }

        public void Dispose()
        {
            Sitecore.Diagnostics.Profiler.EndOperation();
        }
    }
}
