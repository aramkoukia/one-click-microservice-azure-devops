using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace one_click_microservice
{
    class Program
    {
        //Constant value to target Azure DevOps. DO NOT CHANGE
        
        ////============= Config [Edit these with your settings] =====================
        internal const string azureDevOpsResourceId = "499b84ac-1321-427f-aa17-267ca6975798";
        
        ////change to the URL of your Azure DevOps account; NOTE: This must use HTTPS
        internal static string azureDevOpsOrganizationUrl = "http://dev.azure.com/aramkoukia"; // config["azureDevOpsOrganizationUrl"];

        ////change to your app registration's Application ID, unless you are an MSA backed account
        internal static string clientId = "872cd9fa-d31f-45e0-9eab-6e460a02d1f1";  // config["clientId"];

        ////change to your app registration's reply URI, unless you are an MSA backed account
        internal static string replyUri = "urn:ietf:wg:oauth:2.0:oob"; // config["replyUri"]; 
        
        ////==========================================================================

        internal static string personalAccessToken = "put personal access token here";

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            // Create a connection
            //VssConnection connection = new VssConnection(orgUrl, new VssBasicCredential(string.Empty, personalAccessToken));

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
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.Encoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", personalAccessToken))));

                    // todo: fix the rest api
                    using (HttpResponseMessage response = client.GetAsync(
                                "https://dev.azure.com/{organization}/{project}/_apis/build/builds?api-version=5.0").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void CreateAzureDevOpsRepository()
        {
            Console.WriteLine("Create Azure DevOps Repository");

            // Interactively ask the user for credentials, caching them so the user isn't constantly prompted
            VssCredentials creds = new VssClientCredentials
            {
                Storage = new VssClientCredentialStorage()
            };

            VssConnection connection = new VssConnection(new Uri(azureDevOpsOrganizationUrl), new VssBasicCredential(string.Empty, personalAccessToken));

            // Connect to Azure DevOps Services
            // VssConnection connection = new VssConnection(new Uri(c_collectionUri), creds);

            // Get a GitHttpClient to talk to the Git endpoints
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();
            // gitClient.CreateRepositoryAsync();

            // Get data about a specific repository
            //var repo = gitClient.GetRepositoryAsync(c_projectName, c_repoName).Result;
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

        static private async Task ShowWorkItemDetails(VssConnection connection, int workItemId)
        {
            // Get an instance of the work item tracking client
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                // Get the specified work item
                WorkItem workitem = await witClient.GetWorkItemAsync(workItemId);

                // Output the work item's field values
                foreach (var field in workitem.Fields)
                {
                    Console.WriteLine("  {0}: {1}", field.Key, field.Value);
                }
            }
            catch (AggregateException aex)
            {
                VssServiceException vssex = aex.InnerException as VssServiceException;
                if (vssex != null)
                {
                    Console.WriteLine(vssex.Message);
                }
            }
        }
    }
    #endregion
}
