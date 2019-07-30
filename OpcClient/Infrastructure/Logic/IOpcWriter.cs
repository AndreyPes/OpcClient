using Opc.Ua;
using Opc.Ua.Client;

namespace OpcClient.Infrastructure.Logic
{
    public interface IOpcWriter
    {
        void SetExpectedType(Session session, NodeId nodeId);

        Variant WriteValue(Session session, NodeId nodeId, object valueToWrite, TypeInfo typeInfo);
    }
}
