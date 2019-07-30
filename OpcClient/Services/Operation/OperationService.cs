using OpcClient.Logic.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpcClient.Services.Operation
{
    public class OperationService
    {
        public string connectionString { get; private set; }

        public OperationService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Dictionary<string, string>> GetDataByOpertaionTableNameAndId(string tableName, int id, ICollection<string> fields, string databaseName)
        {

            var _schema = new InformationSchemaDA(connectionString);
            var _operationDA = new OperationDA(connectionString);
            var _avalibleTables = await _schema.GetAllTablesNameAsync(null, databaseName);
            var _isOperationCantains = _avalibleTables.Any(x => 0 == string.Compare(x, tableName, true));
            var _fields = await _schema.GetAllFieldFromTableAsync(tableName, new CancellationToken(), false);
            Dictionary<string, string> _operationDictionary = null;
            if (_isOperationCantains)
            {
                _operationDictionary = new Dictionary<string, string>();
                var _operationData = await _operationDA.GetDataByOperationByIdAsync(new CancellationToken(), tableName, id, _fields, false);
                if (_operationData == null)
                    return null;
                foreach (var x in _operationData)
                {
                    _operationDictionary.Add(x.Key, x.Value.ToString());
                }
            }

            return _operationDictionary;
        }
    }
}
