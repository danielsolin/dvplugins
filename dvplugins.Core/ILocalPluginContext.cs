using Microsoft.Xrm.Sdk;

namespace dvplugins.Core
{
   public interface ILocalPluginContext
   {
      IPluginExecutionContext PluginExecutionContext { get; }
      ITracingService TracingService { get; }
      IOrganizationService OrganizationService { get; }
      void Trace(string message);
   }
}
