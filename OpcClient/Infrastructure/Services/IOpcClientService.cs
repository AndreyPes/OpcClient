using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace OpcClient.Infrastructure.Services
{
    public interface IOpcClientService
    {
        ApplicationConfiguration GetApplicationConfiguration();

        ApplicationInstance GetApplicationInstance(ApplicationConfiguration configuration);

        bool CheckSertificate(ApplicationInstance applicationInstance);

        EndpointDescription GetEndpointDescription();

        EndpointConfiguration GetEndpointConfiguration(ApplicationConfiguration applicationConfiguration);

        ConfiguredEndpoint GetConfiguredEndpoint(EndpointDescription endpointDescription, EndpointConfiguration endpointConfiguration);

        Session GetSession(ApplicationConfiguration applicationConfiguration, ConfiguredEndpoint configuredEndpoint);
    }
}
