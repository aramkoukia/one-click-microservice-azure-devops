using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace one_click_microservice
{
    class Program
    {
        //============= Config [Edit these with your settings] =====================
        //change to the URL of your Azure DevOps account; NOTE: This must use HTTPS
        internal const string azureDevOpsOrganizationUrl = "http://dev.azure.com/organization";

        //change to your app registration's Application ID, unless you are an MSA backed account
        internal const string clientId = "872cd9fa-d31f-45e0-9eab-6e460a02d1f1";

        //change to your app registration's reply URI, unless you are an MSA backed account
        internal const string replyUri = "urn:ietf:wg:oauth:2.0:oob";
        //==========================================================================

        //Constant value to target Azure DevOps. DO NOT CHANGE
        internal const string azureDevOpsResourceId = "499b84ac-1321-427f-aa17-267ca6975798";

        static void Main(string[] args)
        {
            AuthenticateWithAzureDevOps();

            CollectServiceInformation();

            CreateAzureDevOpsProject();

            CreateAzureDevOpsRepository();

            CreateMicroserviceServiceTemplate();

            CommitTemplateProject();

            CreateReleasePipeline();

            CreateBuildPipeline();
        }

        private static void AuthenticateWithAzureDevOps()
        {
            Console.WriteLine("Enter Azure DevOps credentials:");
            Console.WriteLine("Authenticate with Azure DevOps Project");
            AuthenticationContext ctx = GetAuthenticationContext(null);
            AuthenticationResult result = null;

            IPlatformParameters promptBehavior = new PlatformParameters();

            try
            {
                //PromptBehavior.RefreshSession will enforce an authn prompt every time. NOTE: Auto will take your windows login state if possible
                result = ctx.AcquireTokenAsync(azureDevOpsResourceId, clientId, new Uri(replyUri), promptBehavior).Result;
                Console.WriteLine("Token expires on: " + result.ExpiresOn);

                var bearerAuthHeader = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                ListProjects(bearerAuthHeader);
            }
            catch (UnauthorizedAccessException)
            {
                // If the token has expired, prompt the user with a login prompt
                result = ctx.AcquireTokenAsync(azureDevOpsResourceId, clientId, new Uri(replyUri), promptBehavior).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", ex.GetType(), ex.Message);
            }
        }

        private static void CollectServiceInformation()
        {
            Console.WriteLine(
                "Collect Required Informaction: " +
                "DevOps ProjectName, " +
                "Repository Name, " +
                "Template Project Language/Type" +
                "Build Configuration (Kubernetes information?)" +
                "Release Configuation (Kubernetes information?)");
        }

        private static void CreateAzureDevOpsProject()
        {
            Console.WriteLine("Create Azure DevOps Project");
        }

        private static void CreateAzureDevOpsRepository()
        {
            Console.WriteLine("Create Azure DevOps Repository");
        }

        private static void CreateMicroserviceServiceTemplate()
        {
            Console.WriteLine("Create Microservice Template Project");
        }

        private static void CommitTemplateProject()
        {
            Console.WriteLine("Commit Template Project to Azure DevOps Repo");
        }

        private static void CreateBuildPipeline()
        {
            Console.WriteLine("Create Azure DevOps Build Pipeline");
        }

        private static void CreateReleasePipeline()
        {
            Console.WriteLine("Create Azure DevOps Release Pipeline");
        }

        #region Helper Methods
        private static AuthenticationContext GetAuthenticationContext(string tenant)
        {
            AuthenticationContext ctx = null;
            if (tenant != null)
                ctx = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);
            else
            {
                ctx = new AuthenticationContext("https://login.windows.net/common");
                if (ctx.TokenCache.Count > 0)
                {
                    string homeTenant = ctx.TokenCache.ReadItems().First().TenantId;
                    ctx = new AuthenticationContext("https://login.microsoftonline.com/" + homeTenant);
                }
            }

            return ctx;
        }

        private static void ListProjects(AuthenticationHeaderValue authHeader)
        {
            // use the httpclient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(azureDevOpsOrganizationUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "ManagedClientConsoleAppSample");
                client.DefaultRequestHeaders.Add("X-TFS-FedAuthRedirect", "Suppress");
                client.DefaultRequestHeaders.Authorization = authHeader;

                // connect to the REST endpoint            
                HttpResponseMessage response = client.GetAsync("_apis/projects?stateFilter=All&api-version=2.2").Result;

                // check to see if we have a succesfull respond
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("\tSuccesful REST call");
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    Console.WriteLine("{0}:{1}", response.StatusCode, response.ReasonPhrase);
                }
            }
        }
    }
    #endregion
}
