using OpcClient.Logic;

namespace OpcClient
{
    //TODO: add ninject module
    public class OpcClient
    {
  
        public static void Run(string pathToXml, string connectionString, string databaseName)
        {

            OpcClientManager.Run(pathToXml, connectionString, databaseName);
   
        }
    }
}
