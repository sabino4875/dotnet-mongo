namespace Api.CoronaVirusStatistics.Domain.Settings
{
    using System;
    public class MongoDBSettings
    {
        public MongoDBSettings(String connectionURI, String databaseName, Int32 port, String userName,
                       String password, Boolean useTLS, String authenticationDatabaseName,
                       String authenticationMechanism, Boolean useAtlas)
        {
            ConnectionURI = connectionURI;
            DatabaseName = databaseName;
            Port = port;
            UserName = userName;
            Password = password;
            UseTLS = useTLS;
            AuthenticationDatabaseName = authenticationDatabaseName;
            AuthenticationMechanism = authenticationMechanism;
            UseAtlas = useAtlas;
        }

        public MongoDBSettings(String connectionURI, String databaseName, Int32 port, String userName, String password, Boolean useAtlas)
            : this(connectionURI: connectionURI, databaseName: databaseName,
                   port: port, userName: userName, password: password, useTLS: false,
                   authenticationDatabaseName: "admin", authenticationMechanism: "SCRAM-SHA-1", useAtlas: useAtlas)
        {
        }


        public String ConnectionURI { get; private set; }
        public String DatabaseName { get; private set; }
        public Int32 Port { get; private set; }
        public String UserName { get; private set; }
        public String Password { get; private set; }
        public Boolean UseTLS { get; private set; }
        public String AuthenticationDatabaseName { get; private set; }
        public String AuthenticationMechanism { get; private set; }
        public Boolean UseAtlas { get; private set; }
    }
}
