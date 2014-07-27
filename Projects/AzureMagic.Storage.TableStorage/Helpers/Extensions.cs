namespace AzureMagic.Storage.TableStorage.Helpers
{
    public static class Extensions
    {
        public static bool IsDevelopmentStorage(this string connectionString)
        {
            return (connectionString == "UseDevelopmentStorage=true;");
        }
    }
}