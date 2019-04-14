using System;

namespace one_click_microservice
{
    class Program
    {
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
    }
}
