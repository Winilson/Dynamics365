using Microsoft.Xrm.Sdk.Client;
using System;
using System.Net;
using System.ServiceModel.Description;

namespace ConsoleLab
{
    public class Conexao
    {
        public OrganizationServiceProxy Obter()
        {
            Uri uri = new Uri("https://labdynwinilson.api.crm.dynamics.com/XRMServices/2011/Organization.svc");

            ClientCredentials clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = "admin@CRM801337.onmicrosoft.com";
            clientCredentials.UserName.Password = "i9TN81Xt0R";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(uri, null, clientCredentials, null);

            serviceProxy.EnableProxyTypes();

            return serviceProxy;
        }
    }
}