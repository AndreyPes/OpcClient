using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpcClient.Infrastructure.Logic
{
    interface IOperationService
    {
         Task<Dictionary<string, string>> GetDataByOpertaionTableNameAndId(string tableName, int id, ICollection<string> fields);

    }
}
