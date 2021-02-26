using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicPlugin
{
    public class ValidateAccountName : IPlugin
    {
        
        private List<string> invalidNames = new List<string>();

        
        public ValidateAccountName(string unsecure)
        {
            
            if (!string.IsNullOrWhiteSpace(unsecure))
                unsecure.Split(',').ToList().ForEach(s => {
                    invalidNames.Add(s.Trim());
                });
        }
        public void Execute(IServiceProvider serviceProvider)
        {

            
            ITracingService tracingService =
              (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            try
            {

                 
                IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

                
                if (context.InputParameters.Contains("Target") && 
                  context.InputParameters["Target"] is Entity && 
                  ((Entity)context.InputParameters["Target"]).LogicalName.Equals("account") && 
                  ((Entity)context.InputParameters["Target"])["name"] != null && 
                  context.MessageName.Equals("Update") && 
                  context.PreEntityImages["a"] != null && 
                  context.PreEntityImages["a"]["name"] != null)
                {
                     
                    var entity = (Entity)context.InputParameters["Target"];
                    var newAccountName = (string)entity["name"];
                    var oldAccountName = (string)context.PreEntityImages["a"]["name"];

                    if (invalidNames.Count > 0)
                    {
                        tracingService.Trace("ValidateAccountName: Teste para {0} nomes inválidos:", invalidNames.Count);

                        if (invalidNames.Contains(newAccountName.ToLower().Trim()))
                        {
                            tracingService.Trace("ValidateAccountName: Teste para {0} nomes inválidos:.", newAccountName);

                            
                            if (!oldAccountName.ToLower().Contains(newAccountName.ToLower().Trim()))
                            {
                                tracingService.Trace("ValidateAccountName: novo nome '{0}' não encontrado em '{1}'.", newAccountName, oldAccountName);

                                string message = string.Format("Você não pode alterar o nome desta conta de '{0}' para '{1}'.", oldAccountName, newAccountName);

                                throw new InvalidPluginExecutionException(message);
                            }

                            tracingService.Trace("ValidateAccountName: novo nome '{0}' encontrado no antigo nome '{1}'.", newAccountName, oldAccountName);
                        }

                        tracingService.Trace("ValidateAccountName: novo nome '{0}' não encontrado em invalidNames.", newAccountName);
                    }
                    else
                    {
                        tracingService.Trace("ValidateAccountName: Nenhum nome inválido passado na configuração.");
                    }
                }
                else
                {
                    tracingService.Trace("ValidateAccountName: A etapa para este plug -in não está configurada corretamente..");
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace("BasicPlugin: {0}", ex.ToString());
                throw;
            }
        }
    }
}