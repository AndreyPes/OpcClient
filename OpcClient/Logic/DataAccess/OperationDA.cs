using Newtonsoft.Json.Linq;
using OpcClient.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpcClient.Logic.DataAccess
{
    public class OperationDA /*: IReportOperationDA*/
    {
        public OperationDA(string connectionString)
        {
            this.connectionString = connectionString;
        }

        readonly string connectionString = string.Empty;

        public async Task<List<JObject>> GetOperationDataAsync(string name, ICollection<string> fields, bool ifIdIsNeed, CancellationToken cancellationToken)
        {
            List<JObject> _operationList = new List<JObject>();
            try
            {      
                using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
                {
                    IInformationSchemaDA _informationSchema = new InformationSchemaDA(connectionString);
                    var _operationFieldsStack = await _informationSchema.GetAllFieldFromTableAsync(name, cancellationToken, ifIdIsNeed);
                    string _fieldNames = string.Empty;
                    if (_operationFieldsStack == null)
                        return null;

                    while (_operationFieldsStack.Count > 0)
                    {
                        if (fields != null && fields.Count > 0 && fields.Any(x => 0 == string.Compare(x, _operationFieldsStack.Peek(), true)))
                            if (_fieldNames.Length > 0)
                                _fieldNames += "," + name + "." + _operationFieldsStack.Pop();
                            else
                                _fieldNames += " " + name + "." + _operationFieldsStack.Pop();
                        else
                            _operationFieldsStack.Pop();
                    }


                    await _sqlConnection.OpenAsync();
                    SqlCommand _sqlCommand = new SqlCommand(@"select " + _fieldNames + " from "+name, _sqlConnection);

                    using (SqlDataReader _sqlDataReader = await _sqlCommand.ExecuteReaderAsync())
                    {
                        while (await _sqlDataReader.ReadAsync())
                        {
                            JObject _operation = new JObject();
                            for (int i = 0; i < _sqlDataReader.FieldCount; i++)
                            {  
                                if (!_sqlDataReader.IsDBNull(_sqlDataReader.GetOrdinal(_sqlDataReader.GetName(i))))
                                    _operation.Add(_sqlDataReader.GetName(i), _sqlDataReader.GetValue(_sqlDataReader.GetOrdinal(_sqlDataReader.GetName(i))).ToString());
                                else                                
                                    _operation.Add(_sqlDataReader.GetName(i), null);                                
                            }
                            _operationList.Add(_operation);
                        }
                    }


                    //SqlCommand _sqlCommand = new SqlCommand(@"select top(1) MachineName from Equipment order by DatatimeUpdate desc", _sqlConnection);
                    //_currentEquipmentName = new JObject();
                    //_currentEquipmentName.Add("MachineName", _sqlCommand.ExecuteScalar().ToString());

                }
            }
            catch (SqlException ex)
            {
               // logger.Log(NLog.LogLevel.Error, "Вызов GetEquipmentNameAsync(" + ifIdIsNeed + " ifIdIsNeed,CancellationToken cancellationToken)");
                throw;
            }
            catch (Exception ex)
            {
               // logger.Log(NLog.LogLevel.Error, "Вызов GetEquipmentNameAsync(" + ifIdIsNeed + " ifIdIsNeed,CancellationToken cancellationToken)");
                throw;
            }

            return _operationList;
        }


        //public async Task<List<JObject>> GetDataByOperationNameAsync(CancellationToken cancellationToken, string tableName)
        //{
        //    List<JObject> dataOperation = null;
        //    SqlCommand _sqlCommand = new SqlCommand();

        //    try
        //    {
        //        using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
        //        {
        //            _sqlCommand.CommandText = @"select id,name from " + tableName;
        //            _sqlCommand.Connection = _sqlConnection;

        //            await _sqlConnection.OpenAsync();

        //            using (SqlDataReader _sqlDataReader = await _sqlCommand.ExecuteReaderAsync())
        //            {
        //                while (await _sqlDataReader.ReadAsync())
        //                {
        //                    var jsonObject = new JObject();
        //                    for (int i = 0; i < _sqlDataReader.FieldCount; i++)
        //                    {
        //                        jsonObject.Add(_sqlDataReader.GetName(i), _sqlDataReader.GetValue(_sqlDataReader.GetOrdinal(_sqlDataReader.GetName(i))).ToString());
        //                    }
        //                    if (dataOperation == null)
        //                        dataOperation = new List<JObject>();
        //                    dataOperation.Add(jsonObject);
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return dataOperation;
        //}


        public async Task<JObject> GetDataByOperationNameAsync(CancellationToken cancellationToken, string tableName, string name, Stack<string> fields, bool ifIdIsNeed)
        {
            JObject _operation = null;
            try
            {
                //name = "report" + name;
                //logger.Log(NLog.LogLevel.Info, "Вызов ReportOperationDA.GetDataByOperationByIdAsync(" + name + " name," + id + " id ," + _reportRowStack + ", _reportRowStack, CancellationToken cancellationToken)");

                using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
                {
                    IInformationSchemaDA _informationSchema = new InformationSchemaDA(connectionString);/* NinjectWebCommon.GetKernel().Get<IInformationSchemaService>("InformationSchemaService");*/
                    var _operationFieldsStack = await _informationSchema.GetAllFieldFromTableAsync(tableName, cancellationToken, ifIdIsNeed);
                    string _fieldNames = string.Empty;
                    if (_operationFieldsStack == null)
                        return null;

                    while (_operationFieldsStack.Count > 0)
                    {
                        if (fields != null && fields.Count > 0 && fields.Any(x => 0 == string.Compare(x, _operationFieldsStack.Peek(), true)))
                            if (_fieldNames.Length > 0)
                                _fieldNames += "," + tableName + "." + fields.Pop();
                            else
                                _fieldNames += " " + tableName + "." + fields.Pop();
                        else
                            _operationFieldsStack.Pop();
                    }


                    if (_fieldNames == null)
                        return null;

                    await _sqlConnection.OpenAsync();
                    SqlCommand _sqlCommand = new SqlCommand(@"select top 1 " + _fieldNames + " from " + tableName + " where id='" + name + "'", _sqlConnection);

                    using (SqlDataReader _sqlDataReader = await _sqlCommand.ExecuteReaderAsync())
                    {

                        while (await _sqlDataReader.ReadAsync())
                        {
                            _operation = new JObject();
                            for (int i = 0; i < _sqlDataReader.FieldCount; i++)
                            {
                                if (!_sqlDataReader.IsDBNull(_sqlDataReader.GetOrdinal(_sqlDataReader.GetName(i))))
                                    _operation.Add(_sqlDataReader.GetName(i), _sqlDataReader.GetValue(_sqlDataReader.GetOrdinal(_sqlDataReader.GetName(i))).ToString());
                                else
                                    _operation.Add(_sqlDataReader.GetName(i), null);
                            }
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                //logger.Log(NLog.LogLevel.Error, "Вызов GetDataByOperationByIdAsync(" + name + " name," + id + " id ," + _reportRowStack + ", _reportRowStack, CancellationToken cancellationToken)");
                throw;
            }
            catch (Exception ex)
            {
                //logger.Log(NLog.LogLevel.Error, "Вызов GetDataByOperationByIdAsync(" + name + " name," + id + " id ," + _reportRowStack + ", _reportRowStack, CancellationToken cancellationToken)");
                throw;
            }

            return _operation;
        }




        public async Task<JObject> GetDataByOperationByIdAsync(CancellationToken cancellationToken, string tableName, int id, Stack<string> fields, bool ifIdIsNeed)
        {
            JObject _operation = null;
            try
            {
                //name = "report" + name;
                //logger.Log(NLog.LogLevel.Info, "Вызов ReportOperationDA.GetDataByOperationByIdAsync(" + name + " name," + id + " id ," + _reportRowStack + ", _reportRowStack, CancellationToken cancellationToken)");

                using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
                {
                    IInformationSchemaDA _informationSchema = new InformationSchemaDA(connectionString);/* NinjectWebCommon.GetKernel().Get<IInformationSchemaService>("InformationSchemaService");*/
                    var _operationFieldsStack = await _informationSchema.GetAllFieldFromTableAsync(tableName, cancellationToken, ifIdIsNeed);
                    string _fieldNames = string.Empty;
                    if (_operationFieldsStack == null)
                        return null;

                    while (_operationFieldsStack.Count > 0)
                    {
                        if (fields != null && fields.Count > 0 && fields.Any(x => 0 == string.Compare(x, _operationFieldsStack.Peek(), true)))
                            if (_fieldNames.Length > 0)
                                _fieldNames += "," + tableName + "." + fields.Pop();
                            else
                                _fieldNames += " " + tableName + "." + fields.Pop();
                        else
                            _operationFieldsStack.Pop();
                    }


                    if (_fieldNames == null)
                        return null;

                    await _sqlConnection.OpenAsync();
                    SqlCommand _sqlCommand = new SqlCommand(@"select top 1 " + _fieldNames + " from " + tableName + " where id='" + id + "'", _sqlConnection);

                    using (SqlDataReader _sqlDataReader = await _sqlCommand.ExecuteReaderAsync())
                    {

                        while (await _sqlDataReader.ReadAsync())
                        {
                            _operation = new JObject();
                            for (int i = 0; i < _sqlDataReader.FieldCount; i++)
                            {
                                if (!_sqlDataReader.IsDBNull(_sqlDataReader.GetOrdinal(_sqlDataReader.GetName(i))))
                                    _operation.Add(_sqlDataReader.GetName(i), _sqlDataReader.GetValue(_sqlDataReader.GetOrdinal(_sqlDataReader.GetName(i))).ToString());
                                else
                                    _operation.Add(_sqlDataReader.GetName(i), null);
                            }
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                //logger.Log(NLog.LogLevel.Error, "Вызов GetDataByOperationByIdAsync(" + name + " name," + id + " id ," + _reportRowStack + ", _reportRowStack, CancellationToken cancellationToken)");
                throw;
            }
            catch (Exception ex)
            {
                //logger.Log(NLog.LogLevel.Error, "Вызов GetDataByOperationByIdAsync(" + name + " name," + id + " id ," + _reportRowStack + ", _reportRowStack, CancellationToken cancellationToken)");
                throw;
            }

            return _operation;
        }







    }
}
