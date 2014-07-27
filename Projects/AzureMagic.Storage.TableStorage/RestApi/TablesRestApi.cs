using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AzureMagic.Storage.TableStorage.RestApi
{
    public class TablesRestApi
    {
        private readonly StorageAccount Account;

        public TablesRestApi(StorageAccount account)
        {
            Account = account;
        }

        public async Task<HttpResponseMessage> ListAsync()
        {
            var now = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, CombineUri(Account.TablesUri, "Tables"));
                var headers = request.Headers;

                headers.Accept.Clear();
                headers.Accept.ParseAdd("application/json;odata=fullmetadata");

                headers.Add("x-ms-date", now);

                // http://msdn.microsoft.com/en-us/library/azure/dd179405.aspx implies this value is optional.
                // However when accept header is application/json then x-ms-version header is required.
                headers.Add("x-ms-version", "2013-08-15");

                const AuthorizationScheme authorizationScheme = AuthorizationScheme.SharedKey;

                var signature = GetSignature(request, Account.Name, Account.Key, authorizationScheme);
                var credentials = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Account.Name, signature);

                headers.Authorization = new AuthenticationHeaderValue(authorizationScheme.ToString(), credentials);

                return await client.SendAsync(request);
            }
        }

        private static Uri CombineUri(Uri uri, string relativeUri)
        {
            var path = uri.PathAndQuery;

            if (path.Contains("?"))
            {
                throw new NotImplementedException("Cannot handle ?");
            }

            if (path != "/")
            {
                relativeUri = path + "/" + relativeUri;
            }

            return new Uri(uri, relativeUri);
        }

        private static string GetSignature(HttpRequestMessage request, string account, string storageKey, AuthorizationScheme authorizationScheme = AuthorizationScheme.SharedKey)
        {
            var sharedKey = Convert.FromBase64String(storageKey);
            var resource = GetResource(request);
            var stringToSign = GetStringToSign(request, account, authorizationScheme, resource);
            var signedSignature = SignSignature(sharedKey, stringToSign);

            return signedSignature;
        }

        private static string SignSignature(byte[] key, string message)
        {
            using (HashAlgorithm hashAlgorithm = new HMACSHA256(key))
            {
                var messageBuffer = Encoding.UTF8.GetBytes(message);
                return Convert.ToBase64String(hashAlgorithm.ComputeHash(messageBuffer));
            }
        }

        private static string GetResource(HttpRequestMessage request)
        {
            var resource = request.RequestUri.PathAndQuery;

            if (resource.Contains("?"))
            {
                throw new NotImplementedException(@"Commented line is correct but GetStringToSign needs the parameters. Refer to D:\Exclude-From-Backups\Code\OpenSource\azure-storage-net\Lib\Common\Core\Util\AuthenticationUtility.cs");
                //resource = resource.Substring(0, resource.IndexOf("?", StringComparison.Ordinal));
            }

            return resource;
        }

        private static string GetStringToSign(HttpRequestMessage request, string account, AuthorizationScheme authorizationScheme, string resource)
        {
            var headers = request.Headers;

            switch (authorizationScheme)
            {
                case AuthorizationScheme.SharedKeyLite:
                    return string.Format("{0}\n/{1}{2}", headers.Date.GetValueOrDefault().ToString("R"), account, resource);

                case AuthorizationScheme.SharedKey:
                    return string.Format("{0}\n{1}\n{2}\n{3}\n/{4}{5}",
                        request.Method,
                        TryGetHeaderValue(headers, "Content-MD5", ""),
                        TryGetHeaderValue(headers, "Content-Type", ""),
                        TryGetHeaderValue(headers, "x-ms-date", ""),
                        account,
                        resource);

                default:
                    throw new NotImplementedException(string.Format("KeyType.{0} has not been implemented.", authorizationScheme));
            }
        }

        private static string TryGetHeaderValue(HttpHeaders headers, string key, string defaultValue)
        {
            IEnumerable<string> values;

            var value = headers.TryGetValues(key, out values) ? values.Single() : defaultValue;

            return value;
        }

        private enum AuthorizationScheme
        {
            SharedKeyLite,
            SharedKey
        };
    }
}