using Opc.Ua;
using Opc.Ua.Configuration;

namespace OpcClient.Logic.Configurations
{
    //todo: add ninject
    internal static class Configuration
    {
        static ApplicationInstance _application;
        static ApplicationInstance AppInstance { get => _application; set => _application = _application ?? value; }

        static ApplicationConfiguration _appConfiguration;
        static ApplicationConfiguration AppConfiguration { get => _appConfiguration; set => _appConfiguration = _appConfiguration ?? value; }


        public static ApplicationConfiguration SetConfigurations()
        {
            if(AppConfiguration is null)
            AppConfiguration = new ApplicationConfiguration()
            {
                ApplicationName = "Izovac Opc Client",
                ApplicationUri = Utils.Format(@"urn:{0}:IzovacOpcClient", System.Net.Dns.GetHostName()),
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = Utils.Format(@"CN={0}, DC={1}", "MyHomework", System.Net.Dns.GetHostName()) },
                    TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Certificate Authorities" },
                    TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Applications" },
                    RejectedCertificateStore = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\RejectedCertificates" },
                    RejectSHA1SignedCertificates = false,
                    AutoAcceptUntrustedCertificates = true,
                    AddAppCertToTrustedStore = true,
                    MinimumCertificateKeySize = 1024
                },

                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
                TraceConfiguration = new TraceConfiguration()
            };

            return AppConfiguration;
        }

        public static ApplicationInstance SetApplicationInstance(ApplicationConfiguration appConfiguration)
        {
            if (AppInstance is null)
                AppInstance = new ApplicationInstance
            {
                ApplicationName = "IzovacOpcClient",
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = appConfiguration
                };

            return AppInstance;
        }
    }
}
