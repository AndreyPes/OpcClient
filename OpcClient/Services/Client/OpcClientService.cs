using Opc.Ua;
using OpcClient.Logic.Configurations;
using Opc.Ua.Configuration;
using Opc.Ua.Client;
using System.Collections.Generic;
using System;

namespace OpcClient.Services.Client
{
    internal class OpcClientService
    {
        //todo: constructor variables into config
        public OpcClientService()
        {
            DiscoveryUrl = "opc.tcp://192.168.13.100:4840";           
            
            UseSecurity = false;//true

            OperationTimeOut = 15000;

            Silent = false;

            MinimumKeySize = 2048;

            UpdateBeforeConnect = false;

            SessionName = "IzoOpcSession";

            UserIdentity = null;

            PreferredLocales = null;

            //SessionTimeout = 6000;
            SessionTimeout = 1;
        }

        string DiscoveryUrl { get; }

        bool UseSecurity { get; }

        int OperationTimeOut { get; }

        bool Silent { get; }

        ushort MinimumKeySize { get; }

        bool UpdateBeforeConnect { get; }

        string SessionName { get; }

        IUserIdentity UserIdentity { get; }

        IList<string> PreferredLocales { get; }

        uint SessionTimeout { get; }

        public ApplicationConfiguration GetApplicationConfiguration()
        {
            try
            {
                var _configuration = Configuration.SetConfigurations();
                _configuration.Validate(ApplicationType.Client).GetAwaiter().GetResult();
                if (_configuration.SecurityConfiguration.AutoAcceptUntrustedCertificates)
                {
                    _configuration.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
                }

                return _configuration;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public ApplicationInstance GetApplicationInstance(ApplicationConfiguration configuration)
        {
            try
            {
                var _applicationInstanceSettings = Configuration.SetApplicationInstance(configuration);

                return _applicationInstanceSettings;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public bool CheckSertificate(ApplicationInstance applicationInstance)
        {
            try
            {
                return applicationInstance.CheckApplicationInstanceCertificate(Silent, MinimumKeySize).GetAwaiter().GetResult();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public EndpointDescription GetEndpointDescription()
        {
            try
            {
                return CoreClientUtils.SelectEndpoint(DiscoveryUrl, UseSecurity, OperationTimeOut);
            }
            catch(Exception)
            {
                throw new ServiceResultException("Connection can't be establishing.");
            }
        }

        public EndpointConfiguration GetEndpointConfiguration(ApplicationConfiguration applicationConfiguration)
        {
            return EndpointConfiguration.Create(applicationConfiguration);
        }

        public ConfiguredEndpoint GetConfiguredEndpoint(EndpointDescription endpointDescription, EndpointConfiguration endpointConfiguration)
        {
            return new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
        }

        public Session GetSession(ApplicationConfiguration applicationConfiguration, ConfiguredEndpoint configuredEndpoint)
        {
            try
            {
                return Session.Create(applicationConfiguration, configuredEndpoint, UpdateBeforeConnect, SessionName, SessionTimeout, UserIdentity, PreferredLocales).GetAwaiter().GetResult();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
