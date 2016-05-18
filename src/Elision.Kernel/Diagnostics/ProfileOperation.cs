using System;

namespace Elision.Diagnostics
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
