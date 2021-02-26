using ConsoleLab;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Net;
using System.ServiceModel.Description;
using System.Linq;

namespace DiscoveryService
{
    public class Program
    {
        static void Main(string[] args)
        {
            Conexao conexao = new Conexao();

            var serviceProxy = conexao.Obter();

            //Create(serviceProxy);

            //Descoberta();

            //RetornarMultiplo(serviceProxy);

            //CriacaoLinq(serviceProxy);

            //UpdateLinq(serviceProxy);

            //ExcluirLinq(serviceProxy);

            //RetrieveMultipleFetch(serviceProxy);

            Console.ReadKey();
        }

        #region Descoberta
        static void Descoberta()
        {
            Uri local = new Uri("https://disco.crm.dynamics.com/XRMServices/2011/Discovery.svc");

            ClientCredentials clientcred = new ClientCredentials();
            clientcred.UserName.UserName = "admin@CRM801337.onmicrosoft.com";
            clientcred.UserName.Password = "i9TN81Xt0R";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            DiscoveryServiceProxy dsp = new DiscoveryServiceProxy(local, null, clientcred, null);
            dsp.Authenticate();

            RetrieveOrganizationsRequest rosreq = new RetrieveOrganizationsRequest();
            rosreq.AccessType = EndpointAccessType.Default;
            rosreq.Release = OrganizationRelease.Current;

            RetrieveOrganizationsResponse r = (RetrieveOrganizationsResponse)dsp.Execute(rosreq);

            foreach (var item in r.Details)
            {
                Console.WriteLine("Nome " + item.UniqueName);
                Console.WriteLine("Nome Exibição " + item.FriendlyName);
                foreach (var endpoint in item.Endpoints)
                {
                    Console.WriteLine(endpoint.Key);
                    Console.WriteLine(endpoint.Value);
                }
            }
        }
        #endregion

        #region Create
        static void Create(OrganizationServiceProxy serviceProxy)
        {
            for (int i = 0; i < 10; i++)
            {
                Guid registro = new Guid();
                Entity entidade = new Entity("account");
                entidade.Attributes.Add("name", "TesteDev " + i.ToString());

                registro = serviceProxy.Create(entidade);

                if (registro != Guid.Empty)
                    Console.WriteLine("Id do Registro criado : " + registro);
                else
                    Console.WriteLine("Registro não criado!");
            }
        }
        #endregion

        #region QueryExpression
        static void RetornarMultiplo(OrganizationServiceProxy serviceProxy)
        {
            QueryExpression queryExpression = new QueryExpression("account");

            queryExpression.Criteria.AddCondition("name", ConditionOperator.BeginsWith, "teste");
            queryExpression.ColumnSet = new ColumnSet(true);
            EntityCollection colecaoEntidades = serviceProxy.RetrieveMultiple(queryExpression);
            foreach (var item in colecaoEntidades.Entities)
            {
                Console.WriteLine(item["name"]);
            }
        }
        #endregion

        #region CriacaoLinq
        static void CriacaoLinq(OrganizationServiceProxy serviceProxy)
        {
            OrganizationServiceContext context = new OrganizationServiceContext(serviceProxy);
            Entity account = new Entity("account");
            account["name"] = "Treinamento Extending";

            context.AddObject(account);
            context.SaveChanges();
        }
        #endregion

        #region UpdateLinq

        static void UpdateLinq(OrganizationServiceProxy serviceProxy)
        {
            OrganizationServiceContext context = new OrganizationServiceContext(serviceProxy);
            var resultados = from a in context.CreateQuery("contact")
                             where ((string)a["firstname"]) == "Maria"
                             select a;

            foreach (var item in resultados)
            {
                item.Attributes["firstname"] = "Winilson de Paula";
                context.UpdateObject(item);
            }
            context.SaveChanges();
        }

        #endregion

        #region ExcluirLinq

        static void ExcluirLinq(OrganizationServiceProxy serviceProxy)
        {
            OrganizationServiceContext context = new OrganizationServiceContext(serviceProxy);

            var resultados = from a in context.CreateQuery("account")
                             where (string)a["name"] == "Treinamento Extending"
                             select a;

            foreach (var item in resultados)
            {
                context.DeleteObject(item);
            }

            context.SaveChanges();
        }

        #endregion

        #region RetrieveMultipleFetch
        static void RetrieveMultipleFetch(OrganizationServiceProxy _serviceProxy)
        {

            string fetch2 = @"  
                  <fetch mapping='logical'>  
                  <entity name='account'>   
                  <attribute name='accountid'/>   
                  <attribute name='name'/>   
                  <link-entity name='systemuser' to='owninguser'>   
                  <filter type='and'>   
                  <condition attribute='lastname' operator='ne' value='Cannon' />   
                  </filter>   
                  </link-entity>   
                  </entity>   
                  </fetch> ";

            EntityCollection result = _serviceProxy.RetrieveMultiple(new FetchExpression(fetch2));
            foreach (var c in result.Entities)
            {
                System.Console.WriteLine(c.Attributes["name"]);
            }
        }

        #endregion
    }
}