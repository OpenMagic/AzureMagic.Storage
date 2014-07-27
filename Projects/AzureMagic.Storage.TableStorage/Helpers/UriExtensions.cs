using System;

namespace AzureMagic.Storage.TableStorage.Helpers
{
    // todo: docs
    // todo: write specs for all internal method then turn public.
    internal static class UriExtensions
    {
        internal static Uri Append(this Uri uri, string relativeUri)
        {
            var path = uri.PathAndQuery;

            if (path.Contains("?"))
            {
                throw new NotSupportedException("Query strings are current not supported.");
            }

            if (path != "/")
            {
                relativeUri = string.Format("{0}/{1}", path, relativeUri);
            }

            return new Uri(uri, relativeUri);
        }
    }
}