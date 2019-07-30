namespace OpcClient.Infrastructure.Logic
{
    public interface IOpcClientManager
    {
        void Run(string pathToXml, string connectionString, string databaseName);
    }
}
