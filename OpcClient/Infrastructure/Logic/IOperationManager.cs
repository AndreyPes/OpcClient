using Opc.Ua;
using OpcClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlManager.Models;

namespace OpcClient.Infrastructure.Logic
{
    public interface IOperationManager
    {
        Dictionary<string, OpcOperation> GetOpcOperationCollection(List<Operation> operations, ReferenceDescriptionCollection references);
    }
}
