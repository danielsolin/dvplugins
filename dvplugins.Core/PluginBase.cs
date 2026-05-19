using System;
using System.ServiceModel;

using Microsoft.Xrm.Sdk;

namespace dvplugins.Core
{
   public abstract class PluginBase : IPlugin
   {
      protected string PluginClassName { get; }

      protected PluginBase(Type pluginType)
      {
         PluginClassName = pluginType.FullName;
      }

      protected abstract void ExecuteDataversePlugin(
         ILocalPluginContext localContext
      );

      public void Execute(IServiceProvider serviceProvider)
      {
         if(serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

         var localContext = new LocalPluginContext(serviceProvider);

         localContext.Trace($"Entered {PluginClassName}.Execute()");

         try
         {
            ExecuteDataversePlugin(localContext);
         }
         catch(FaultException<OrganizationServiceFault> ex)
         {
            localContext.Trace($"Exception: {ex.ToString()}");

            throw;
         }
         catch(Exception ex)
         {
            localContext.Trace($"Exception: {ex.ToString()}");

            throw new InvalidPluginExecutionException(
               $"An error occurred in {PluginClassName}.",
               ex
            );
         }
         finally
         {
            localContext.Trace($"Exiting {PluginClassName}.Execute()");
         }
      }
   }
}
