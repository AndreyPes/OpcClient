using System;
using Opc.Ua;
using Opc.Ua.Client;

namespace OpcClient.Logic
{
    public class OpcWriter
    {
        public static TypeInfo _sourceType { get; private set; }

        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static void SetExpectedType(Session session, NodeId nodeId)
        {
            try
            {
                ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

                foreach (uint attributeId in new uint[] { Attributes.DataType, Attributes.ValueRank })
                {
                    ReadValueId nodeToRead = new ReadValueId();
                    nodeToRead.NodeId = nodeId;
                    nodeToRead.AttributeId = attributeId;
                    nodesToRead.Add(nodeToRead);
                }

                DataValueCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                session.Read(
                    null,
                    0,
                    TimestampsToReturn.Neither,
                    nodesToRead,
                    out results,
                    out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToRead);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

                NodeId dataTypeId = results[0].GetValue<NodeId>(null);
                int valueRank = results[1].GetValue<int>(ValueRanks.Scalar);

                BuiltInType builtInType = DataTypes.GetBuiltInType(dataTypeId, session.NodeCache.TypeTree);

                _sourceType = new TypeInfo(builtInType, valueRank);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error WriteValue method: {0}", ex.Message);
                throw;
            }
        }

        public static Variant WriteValue(Session session, NodeId nodeId, object valueToWrite, TypeInfo typeInfo)
        {

            try
            {
                object value = TypeInfo.Cast(valueToWrite, typeInfo, _sourceType.BuiltInType);

                WriteValueCollection nodesToWrite = new WriteValueCollection();

                WriteValue nodeToWrite = new WriteValue();
                nodeToWrite.NodeId = nodeId;
                nodeToWrite.AttributeId = Attributes.Value;

                nodeToWrite.Value.WrappedValue = new Variant(value, _sourceType);

                nodesToWrite.Add(nodeToWrite);

                RequestHeader requestHeader = new RequestHeader();
                requestHeader.ReturnDiagnostics = (uint)DiagnosticsMasks.All;

                StatusCodeCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                ResponseHeader responseHeader = session.Write(
                    requestHeader,
                    nodesToWrite,
                    out results,
                    out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToWrite);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToWrite);

                if (StatusCode.IsBad(results[0]))
                {
                    throw ServiceResultException.Create(results[0], 0, diagnosticInfos, responseHeader.StringTable);
                }

                return nodeToWrite.Value.WrappedValue;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error WriteValue method: {0}", ex.Message);
                throw;
            }
        }


    }
}
