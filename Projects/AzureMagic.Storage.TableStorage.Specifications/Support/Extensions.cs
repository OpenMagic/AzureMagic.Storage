namespace AzureMagic.Storage.TableStorage.Specifications.Support
{
    public static class Extensions
    {
        public static string FormatGivenValue(this string value)
        {
            switch (value)
            {
                case "null":
                    return null;

                case "empty":
                    return string.Empty;

                default:
                    return value;
            }
        }
    }
}
