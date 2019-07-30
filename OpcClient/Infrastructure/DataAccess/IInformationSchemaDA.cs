using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpcClient.Infrastructure.DataAccess
{
    public interface IInformationSchemaDA
    {

        Task<Stack<string>> GetAllFieldFromTableAsync(string tableName, CancellationToken cancellationToken, bool ifIdIsNeed);

        Task<Stack<string>> GetAllTablesNameAsync(ICollection<string> notUsedTableName, string databaseName);

    }
}
