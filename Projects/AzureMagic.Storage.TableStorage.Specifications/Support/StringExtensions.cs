using System;

namespace AzureMagic.Storage.TableStorage.Specifications.Support
{
    public static class StringExtensions
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

        public static T FormatGivenValue<T>(this string value, Func<string, T> notNullFunc)
        {
            switch (value)
            {
                case "null":
                    return default(T);

                case "not null":
                    return notNullFunc(value);

                default:
                    return notNullFunc(value);
            }
        }
    }
}