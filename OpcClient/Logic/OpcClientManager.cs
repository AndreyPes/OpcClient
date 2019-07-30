using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using OpcClient.Infrastructure.Logic;
using OpcClient.Logic.DataAccess;
using OpcClient.Models;
using OpcClient.Services.Client;
using OpcClient.Services.Operation;
using XmlManager.Logic.XmlSerializer;
using OpcConverter = OpcClient.Logic.TypeConverter;

namespace OpcClient.Logic
{
    public class OpcClientManager
    {
        static ReferenceDescriptionCollection _references;

        static Byte[] _continuationPoint;

        static ApplicationConfiguration _applicationConfiguration;

        static ApplicationInstance _applicationInstance;

        static bool _ifSertificate;

        static EndpointConfiguration _endpointConfiguration;

        static EndpointDescription _endpointDescription;

        static ConfiguredEndpoint _configuredEndPoint;

        static List<string> _branchs;

        static Session _session;

        static Dictionary<string, OpcOperation> _operationTriggers;

        static OperationManager _operationManager;

        static string _connectionString;

        static NLog.Logger _logger;

        static string _databaseName;

        static OpcClientManager()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }


        public static void Run(string pathToXml, string connectionString, string databaseName)
        {
            try
            {
                var _opcClient = new OpcClientService();
                if (pathToXml == null || pathToXml.Length == 0)
                    throw new ArgumentException();
                _connectionString = connectionString;
                _databaseName = databaseName;
                var _operations = XmlSerializer.GetOperations(pathToXml);
                _operationTriggers = new Dictionary<string, OpcOperation>();
                _branchs = new List<string>() { "plc1", "hmi_recipe" };
                _operationManager = new OperationManager(_logger);

                _applicationConfiguration = _applicationConfiguration ?? _opcClient.GetApplicationConfiguration();
                _applicationInstance = _applicationInstance ?? _opcClient.GetApplicationInstance(_applicationConfiguration);
                _ifSertificate = _opcClient.CheckSertificate(_applicationInstance);
                _endpointConfiguration = _endpointConfiguration ?? _opcClient.GetEndpointConfiguration(_applicationConfiguration);
                try
                {
                    _endpointDescription = _endpointDescription ?? _opcClient.GetEndpointDescription();
                }
                catch (ServiceResultException ex)
                {
                    throw ex;
                }
                _configuredEndPoint = _configuredEndPoint ?? _opcClient.GetConfiguredEndpoint(_endpointDescription, _endpointConfiguration);
                _session = _opcClient.GetSession(_applicationConfiguration, _configuredEndPoint);

                if (!_session.Connected)
                {
                    Thread.Sleep(10000);
                    return;
                }

                if (_session == null)
                {
                    _session.KeepAliveInterval = 2000;
                    _session.KeepAlive += new KeepAliveEventHandler(Session_KeepAlive);
                }

                SearchDataInTree(_session, ObjectIds.ObjectsFolder);
                foreach (var s in _branchs)
                {
                    var _element = s;
                    SearchDataInTree(_session, new NodeId((_references.FirstOrDefault(x => 0 == string.Compare(x.DisplayName.ToString(), _element, true)).NodeId.ToString())));
                }

                _operationTriggers = _operationManager.GetOpcOperationCollection(_operations, _references);
                var _operationsListForSubscribe = _references.FindAll(x => true == _operationTriggers.Any(z => 0 == string.Compare(z.Value.key, x.DisplayName.ToString(), true)));
                var _subscription = new Subscription(_session.DefaultSubscription) { PublishingInterval = 1000 };
                var _monitoredItems = Subscribe(_subscription, _operationsListForSubscribe);
                _monitoredItems.ForEach(i => i.Notification += OnNotification);
                Console.WriteLine("Step 1 - Add a list of items you wish to monitor to the subscription.");
                _subscription.AddItems(_monitoredItems);
                Console.WriteLine("Step 2 - Add the subscription to the session.");
                _session.AddSubscription(_subscription);
                _subscription.Create();

                Console.WriteLine("===================//--//=========================");

                Console.ReadKey(true);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error SearchDataInTree method: {0}", ex.Message);
            }
        }

        private static void SearchDataInTree(Session session, NodeId nodeId)
        {
            try
            {
                session.Browse(null,
                      null,
                      nodeId,
                      0u,
                      BrowseDirection.Forward,
                      ReferenceTypeIds.HierarchicalReferences,
                      true,
                      (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method,
                      out _continuationPoint, out _references);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error SearchDataInTree method: {0}", ex.Message);
            }
        }

        private static async void OnNotification(MonitoredItem item, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                foreach (var value in item.DequeueValues())
                {
                    _logger.Info("{0}: {1}, {2}, {3}, {4}", item.DisplayName, item.ResolvedNodeId, value.Value, value.SourceTimestamp, value.StatusCode);
                    Console.WriteLine("{0}: {1}, {2}, {3}, {4}", item.DisplayName, item.ResolvedNodeId, value.Value, value.SourceTimestamp, value.StatusCode);
                    if (value.WrappedValue.TypeInfo.BuiltInType == BuiltInType.Boolean)
                        if ((bool)value.Value == true)
                        {

                            var _currentoperationData = _operationTriggers.FirstOrDefault(x => x.Value.nodeId == item.ResolvedNodeId);
                            if (_currentoperationData.Key is null)
                                break;
                            var _id = _session.ReadValue(_currentoperationData.Value.nodeIdNumSet);

                            var _operation = _currentoperationData.Key;
                            var _operationId = Int32.Parse(_id.ToString());
                            if (_operationId < 1)
                            {
                                _logger.Info("Operation: {0}; Id: {1}", _operation, _id);
                                _logger.Info("Id was less than 1");
                                return;
                            }

                            var _operationService = new OperationService(_connectionString);
                            var _schema = new InformationSchemaDA(_connectionString);
                            var _fields = await _schema.GetAllFieldFromTableAsync(_operation, new CancellationToken(), false);
                            var _result = await _operationService.GetDataByOpertaionTableNameAndId(_operation, _operationId, _fields.ToList(), _databaseName);
                            if (_result == null)
                                return;

                            _logger.Info("======================================//--//======================================");
                            Console.WriteLine("{0}: {1}, {2}, {3}, {4}", item.DisplayName, item.ResolvedNodeId, value.Value, value.SourceTimestamp, value.StatusCode);
                            _logger.Info("Operation: {0}; Id: {1}", _operation, _id);
                            _logger.Info("{0}: {1}, {2}, {3}, {4}", item.DisplayName, item.ResolvedNodeId, value.Value, value.SourceTimestamp, value.StatusCode);

                            var _selectedOperation = _operationTriggers.FirstOrDefault(x => 0 == string.Compare(x.Key, _operation, true));

                            foreach (var s in _selectedOperation.Value.items)
                            {
                                if (s.nodeId == null || s.nodeId == 0)
                                    continue;

                                var _node = _result.FirstOrDefault(x => 0 == string.Compare("recipe_" + _operation + "_" + x.Key, s.item, true));
                                if (_node.Key != null)
                                {

                                    var _value = OpcConverter.Convert(s.typeInfo.ToString(), _node.Value);
                                    Console.WriteLine("{0}: was updated, value:{1}", _node.Key, _value);

                                    OpcWriter.SetExpectedType(_session, s.nodeId);
                                    OpcWriter.WriteValue(_session, s.nodeId, TypeConverter.Convert(s.typeInfo.ToString(), _node.Value), TypeConverter.Convert(s.typeInfo.ToString()));
                                    _logger.Info("{0}: was updated, value:{1}", _node.Key, _value);

                                }
                            }

                            OpcWriter.SetExpectedType(_session, (_currentoperationData.Value.nodeIdNumGet));
                            OpcWriter.WriteValue(_session, _currentoperationData.Value.nodeIdNumGet, OpcConverter.Convert("long", _id.ToString()), OpcConverter.Convert("long"));
                            Console.WriteLine("Id InumGet: was updated on {0}.", _id);
                            _logger.Info("Id InumGet: was updated on {0}.", _id);

                        }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error OnNotification method: {0}", ex.Message);
            }
        }


        private static List<MonitoredItem> Subscribe(Subscription subscription, List<ReferenceDescription> searchedElementsResult)
        {
            var _monitoredItems = new List<MonitoredItem>();

            foreach (var s in searchedElementsResult)
            {
                MonitoredItem _monitoredItem = null;
                if (s.NodeClass == NodeClass.Variable)
                {
                    var _nodeId = (NodeId)s.NodeId;
                    _monitoredItem = new MonitoredItem(subscription.DefaultItem) { DisplayName = "ST1", StartNodeId = _nodeId };
                    _monitoredItems.Add(_monitoredItem);
                }
                if (s.NodeClass == NodeClass.Object)
                {
                    var _nodeId = s.NodeId.ToString() + ".ControlCmd";
                    _monitoredItem = new MonitoredItem(subscription.DefaultItem) { DisplayName = "ST1", StartNodeId = _nodeId };
                    _monitoredItems.Add(_monitoredItem);
                }
            }

            return _monitoredItems;
        }

        //TODO: reconnect
        static void Session_KeepAlive(Session session, KeepAliveEventArgs e)
        {
            try
            {
                if (session.KeepAliveStopped)
                {
                    session.Reconnect();
                }

                if (Object.ReferenceEquals(_session, session))
                {
                    return;
                }

                if (!session.Connected)
                {
                    session.Reconnect();
                    Console.WriteLine("was reconnected!");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error can't reconnect to session: {0}", ex.Message);
            }

        }
    }
}
