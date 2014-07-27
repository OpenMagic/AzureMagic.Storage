using AzureMagic.Storage.TableStorage.Helpers;

namespace AzureMagic.Storage.TableStorage.Specifications.Support.Contexts
{
    public class RestApiContext
    {
        public readonly string ConnectionString;
        public readonly bool UsingDevelopmentStorage;

        public RestApiContext()
        {
            // Future version may allow connection to Azure account.
            ConnectionString = "UseDevelopmentStorage=true;";
            
            if (ConnectionString.IsDevelopmentStorage())
            {
                WindowAzureStorageEmulatorManager.StartEmulator();
            }
        }
    }
}
