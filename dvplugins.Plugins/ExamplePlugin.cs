using System;
using Microsoft.Xrm.Sdk;

using dvplugins.Core;

namespace dvplugins.Plugins
{
   public class ExamplePlugin : PluginBase
   {
      public ExamplePlugin(Type pluginType) : base(pluginType)
      {
      }

      protected override void ExecuteDataversePlugin(
         ILocalPluginContext localContext
      )
      {
         var context = localContext.PluginExecutionContext;

         if(context.MessageName != "MESSAGENAME" ||
            context.InputParameters.Contains("Target") == false
         )
         {
            return;
         }

         if(context.InputParameters["Target"] is Entity target
            && target.LogicalName == "ENTITYLOGICALNAME")
         {
            localContext.Trace("Here we would start doing something...");

            // Do something.
         }
      }
   }
}
