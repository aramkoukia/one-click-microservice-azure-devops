using System;

namespace one_click_microservice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Authenticate with Azure DevOps Project");
            Console.WriteLine(
                "Collect Required Informaction: " +
                "DevOps ProjectName, " +
                "Repository Name, " +
                "Template Project Language/Type" +
                "Build Configuration (Kubernetes information?)" +
                "Release Configuation (Kubernetes information?)");

            Console.WriteLine("Create Azure DevOps Project");
            Console.WriteLine("Create DevOps Repo");
            Console.WriteLine("Create Template Project");
            Console.WriteLine("Commit Template Project to Azure DevOps Repo");
            Console.WriteLine("Create DevOps Release Pipeline");
            Console.WriteLine("Create DevOps Build Pipeline");
        }
    }
}
