namespace Api.CoronaVirusStatistics.InfraStructure.Context
{
    using Api.CoronaVirusStatistics.Domain.Settings;
    using MongoDB.Driver;
    using System;
    using System.Security.Authentication;
    using System.Xml.Xsl;

    public class ApplicationDBContext : IDisposable
    {
        private Boolean _disposable;
        public IMongoDatabase Database { get; private set; }

        public ApplicationDBContext(MongoDBSettings config)
        {
            _disposable = true;
            MongoClientSettings settings = new MongoClientSettings();

            if (config.UseAtlas) 
            {
                //Access mongodb atlas
                var connectionUri = $"mongodb+srv://{config.UserName}:{config.Password}@{config.ConnectionURI}/?retryWrites=true&w=majority";
                settings = MongoClientSettings.FromConnectionString(connectionUri);
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            }
            else
            {
                //Access managed server
                settings.Server = new MongoServerAddress(config.ConnectionURI, config.Port);
                settings.UseTls = config.UseTLS;
                settings.SslSettings = new SslSettings();
                settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

                MongoIdentity identity = new MongoInternalIdentity(config.AuthenticationDatabaseName, config.UserName);
                MongoIdentityEvidence evidence = new PasswordEvidence(config.Password);
                settings.Credential = new MongoCredential(config.AuthenticationMechanism, identity, evidence);
            }

            MongoClient client = new MongoClient(settings);
            Database = client.GetDatabase(config.DatabaseName);

        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing && _disposable)
            {
                if (Database != null)
                {
                    Database = null;
                }
                _disposable = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ApplicationDBContext()
        {
            Dispose(false);
        }
    }
}
