using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpcClient.Infrastructure.DataAccess
{
    public interface IOperationDA
    {
        Task<List<JObject>> GetOperationDataAsync(string name, ICollection<string> fields, bool ifIdIsNeed, CancellationToken cancellationToken);

        Task<JObject> GetDataByOperationNameAsync(CancellationToken cancellationToken, string tableName, string name, Stack<string> fields, bool ifIdIsNeed);

        Task<JObject> GetDataByOperationByIdAsync(CancellationToken cancellationToken, string tableName, int id, Stack<string> fields, bool ifIdIsNeed);
    }
}
