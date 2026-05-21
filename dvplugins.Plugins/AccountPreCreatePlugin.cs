using System;
using Microsoft.Xrm.Sdk;

using dvplugins.Core;

namespace dvplugins.Plugins
{
   public class AccountPreCreatePlugin : PluginBase
   {
      public AccountPreCreatePlugin(Type pluginType) : base(pluginType)
      {
      }

      protected override void ExecuteDataversePlugin(
         ILocalPluginContext localContext
      )
      {
         var context = localContext.PluginExecutionContext;

         if(context.MessageName != "Create" ||
            context.InputParameters.Contains("Target") == false
         )
         {
            return;
         }

         if(context.InputParameters["Target"] is Entity target
            && target.LogicalName == "account")
         {
            localContext.Trace("Processing account creation...");

            if(!target.Contains("description"))
            {
               target["description"] = "Default description set by " +
                  "dvplugins.Core framework.";

               localContext.Trace("Description was empty. Set default value.");
            }
         }
      }
   }
}
