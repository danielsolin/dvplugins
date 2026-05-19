using Microsoft.Xrm.Sdk;

using System;

namespace dvplugins.Core
{
   public class LocalPluginContext : ILocalPluginContext
   {
      public IPluginExecutionContext PluginExecutionContext { get; }
      public ITracingService TracingService { get; }
      public IOrganizationService OrganizationService { get; }

      public LocalPluginContext(IServiceProvider serviceProvider)
      {
         if(serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

         PluginExecutionContext = (IPluginExecutionContext)serviceProvider
                .GetService(typeof(IPluginExecutionContext));

         TracingService = (ITracingService)serviceProvider
                .GetService(typeof(ITracingService));

         var factory = (IOrganizationServiceFactory)serviceProvider
                .GetService(typeof(IOrganizationServiceFactory));

         OrganizationService = factory.CreateOrganizationService(
            PluginExecutionContext.UserId
         );
      }

      public void Trace(string message)
      {
         if(string.IsNullOrWhiteSpace(message) || TracingService == null)
            return;

         TracingService.Trace(message);
      }
   }
}
