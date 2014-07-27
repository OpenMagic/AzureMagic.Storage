using System;

namespace AzureMagic.Storage.TableStorage
{
    // todo: docs
    public class StorageAccount
    {
        public StorageAccount(string accountName, string accountKey)
            : this(accountName, accountKey, CreateTablesUri(accountName))
        {
        }

        public StorageAccount(string accountName, string accountKey, Uri tablesUri)
        {
            Name = accountName;
            Key = accountKey;
            TablesUri = tablesUri;

            ConnectionString = accountName =="devstoreaccount1" 
                ? "UseDevelopmentStorage=true;" 
                : string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};", accountName, accountKey);
        }

        public string Name { get; private set; }
        public string Key { get; private set; }
        public Uri TablesUri { get; private set; }
        public string ConnectionString { get; private set; }

        public static Uri CreateTablesUri(string accountName)
        {
            return new Uri(string.Format("https://{0}.table.core.windows.net/", accountName));
        }

        public static StorageAccount ForStorageEmulator()
        {
            return new StorageAccount(
                "devstoreaccount1",
                "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==",
                new Uri("http://127.0.0.1:10002/devstoreaccount1"));
        }

        public static StorageAccount FromConnectionString(string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}