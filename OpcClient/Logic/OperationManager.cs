using Opc.Ua;
using OpcClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using XmlManager.Models;

namespace OpcClient.Logic
{
    public class OperationManager
    {
        public OperationManager(NLog.Logger logger)
        {
            _logger = logger;
        }

         NLog.Logger _logger;

        public Dictionary<string, OpcOperation> GetOpcOperationCollection(List<Operation> operations, ReferenceDescriptionCollection references)
        {
            try
            {
                Dictionary<string, OpcOperation> _operationTriggers = operations.ToDictionary(x => x.Name, x => new OpcOperation("Recipe_" + x.Name + "_xload",
                new NodeId((references.FirstOrDefault(n => 0 == string.Compare(n.DisplayName.ToString(), "Recipe_" + x.Name + "_xload", true))?.NodeId.ToString())),
                new NodeId((references.FirstOrDefault(n => 0 == string.Compare(n.DisplayName.ToString(), "Recipe_" + x.Name + "_iNumSet", true))?.NodeId.ToString())),
                new NodeId((references.FirstOrDefault(n => 0 == string.Compare(n.DisplayName.ToString(), "Recipe_" + x.Name + "_iNumGet", true))?.NodeId.ToString())),
                x.Items.Item.Select(z => new OpcOperationItem("Recipe_" + x.Name + "_" + z.Name, z.Type,
                new NodeId((references.FirstOrDefault(n => 0 == string.Compare(n.DisplayName.ToString(), "Recipe_" + x.Name + "_" + z.Name, true))?.NodeId.ToString()))))));

                return _operationTriggers;
            }
            catch (NullReferenceException ex)
            {
                _logger.Error("Error _operationTriggers collection can't be used: {0}", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error _operationTriggers collection can't be used: {0}", ex.Message);
                throw ex;
            }
        }

    }
}
