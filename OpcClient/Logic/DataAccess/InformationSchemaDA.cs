using OpcClient.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace OpcClient.Logic.DataAccess
{
    public class InformationSchemaDA : IInformationSchemaDA
    {
        //public InformationSchemaDA()
        //{
        //    this.connectionString =  NinjectWebCommon.GetKernel().Get<ConnectionConfiguration>("ConnectionStringSQL").ConnectionString;
        //}

        public InformationSchemaDA(string connectionString)
        {
            this.connectionString = connectionString;
        }


        string connectionString = string.Empty;

        public async Task<Stack<string>> GetAllFieldFromTableAsync(string tableName, CancellationToken cancellationToken, bool ifIdIsNeed)
        {
            Stack<string> _fields = null;
            SqlCommand _sqlCommand = new SqlCommand();


            try
            {
                using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
                {
                    _sqlCommand.Connection = _sqlConnection;

                    if (!ifIdIsNeed)
                    {
                        _sqlCommand.CommandText = @"SELECT distinct COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' and COLUMN_NAME !='id'";
                    }
                    else
                    {
                        _sqlCommand.CommandText = @"SELECT distinct COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "'";
                    }

                    await _sqlConnection.OpenAsync();
                    using (SqlDataReader _sqlDataReader = await _sqlCommand.ExecuteReaderAsync())
                    {
                        while (await _sqlDataReader.ReadAsync())
                        {
                            if (_fields == null)
                                _fields = new Stack<string>();
                            _fields.Push((string)_sqlDataReader[0]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //logger.Log(NLog.LogLevel.Error, "sql error:\r\n"+ex.Message);
                //logger.Log(NLog.LogLevel.Error, "sql error:\r\n" + ex.ToString());
            }
            catch (Exception ex)
            {
                //logger.Log(NLog.LogLevel.Error, "error:\r\n" + ex.Message);
                //logger.Log(NLog.LogLevel.Error, "error:\r\n" + ex.ToString());
            }

            return _fields;
        }


        public async Task<Stack<string>> GetAllTablesNameAsync(ICollection<string> notUsedTableName, string databaseName)
        {
            Stack<string> _operationsTables = null;
            try
            {
                SqlCommand _sqlCommand = new SqlCommand();
                string notUsedTableNameQuery = string.Empty;

                if (notUsedTableName!=null && notUsedTableName.Count > 0)
                    foreach (var s in notUsedTableName)
                    {
                        notUsedTableNameQuery += " and TABLE_NAME != '" + s + "'";
                    }

                using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
                {
                    _sqlCommand.CommandText = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE ='BASE TABLE' and TABLE_CATALOG = '" + databaseName + "'" + notUsedTableNameQuery;
                    _sqlCommand.Connection = _sqlConnection;
                    await _sqlConnection.OpenAsync();
                    using (SqlDataReader _sqlDataReader = await _sqlCommand.ExecuteReaderAsync())
                    {
                        while (await _sqlDataReader.ReadAsync())
                        {
                            if (_operationsTables == null)
                                _operationsTables = new Stack<string>();
                            _operationsTables.Push((string)_sqlDataReader[0]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }

            return _operationsTables;
        }

    }
}
