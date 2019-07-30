using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpcClient.Models
{
    public class OpcOperation
    {
        public OpcOperation(string key, NodeId nodeId, NodeId nodeIdNumSet, NodeId nodeIdNumGet, IEnumerable<OpcOperationItem> items)
        {

            this.key = key;

            this.items = items;

            this.nodeId = nodeId;

            this.nodeIdNumSet = nodeIdNumSet;

            this.nodeIdNumGet = nodeIdNumGet;

        }

        public string key { get; private set; }

        public NodeId nodeId { get; private set; }

        public NodeId nodeIdNumSet { get; private set; }

        public NodeId nodeIdNumGet { get; private set; }

        public IEnumerable<OpcOperationItem> items { get; private set; }

    }

    public class OpcOperationItem
    {

        public OpcOperationItem(string item, /*TypeInfo*/ string typeInfo, NodeId nodeId)
        {

            this.item = item;

            this.nodeId = nodeId;

            this.typeInfo = typeInfo;

        }

        public string item { get; private set; }

        public NodeId nodeId { get; private set; }

        public /*TypeInfo*/ string typeInfo { get; private set; }

    }

}
