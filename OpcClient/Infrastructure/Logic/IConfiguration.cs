using Opc.Ua;
using Opc.Ua.Configuration;

namespace OpcClient.Infrastructure.Logic
{
    public interface IConfiguration
    {
        ApplicationInstance SetApplicationInstance(ApplicationConfiguration appConfiguration);

        ApplicationConfiguration SetConfigurations();
    }
}
