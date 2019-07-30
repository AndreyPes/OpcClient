using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpcClient.Infrastructure.Services
{
    public interface IOperationService
    {
        Task<Dictionary<string, string>> GetDataByOpertaionTableNameAndId(string tableName, int id, ICollection<string> fields, string databaseName);
    }
}
