using System;

namespace Elision.Foundation.Kernel.Diagnostics
{
    public class TraceOperation : IDisposable
    {
        private readonly ProfileOperation _profileOperation;

        public TraceOperation(string name, bool profile = true)
        {
            Sitecore.Diagnostics.Tracer.Info(name);
            Sitecore.Diagnostics.Tracer.Indent++;

            if (profile)
                _profileOperation = new ProfileOperation(name);
        }

        public void Info(object message)
        {
            Sitecore.Diagnostics.Tracer.Info(message);
        }

        public void Debug(object message)
        {
            Sitecore.Diagnostics.Tracer.Debug(message);
        }

        public void Warning(object message)
        {
            Sitecore.Diagnostics.Tracer.Warning(message);
        }

        public void Error(object message)
        {
            Sitecore.Diagnostics.Tracer.Error(message);
        }

        public void Dispose()
        {
            Sitecore.Diagnostics.Tracer.Indent--;
            _profileOperation?.Dispose();
        }
    }
}
