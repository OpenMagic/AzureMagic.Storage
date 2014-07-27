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

        private enum KeyType
        {
            SharedKeyLite,
            SharedKey
        };

        public TablesRestApi(StorageAccount account)
        {
            Account = account;
        }

        public async Task<HttpResponseMessage> ListAsync()
        {
            var now = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

            using (var client = new HttpClient())
            {

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(Account.TablesUri, "Tables"));
                var headers = request.Headers;

                headers.Accept.Clear();
                headers.Accept.ParseAdd("application/json;odata=fullmetadata");

                headers.Date = DateTime.Now;

                // http://msdn.microsoft.com/en-us/library/azure/dd179405.aspx implies this value is optional.
                // However when accept header is application/json then x-ms-version header is required.
                headers.Add("x-ms-version", "2013-08-15");

                headers.Add("Authorization", GetAuthorizationHeader(request, Account.Name, Account.Key));

                return await client.SendAsync(request);
            }
        }

        private static string GetAuthorizationHeader(HttpRequestMessage request, string account, string storageKey, KeyType keyType = KeyType.SharedKeyLite)
        {
            var sharedKey = Convert.FromBase64String(storageKey);
            var resource = GetResource(request);
            var stringToSign = GetStringToSign(request, account, keyType, resource);
            var hasher = new HMACSHA256(sharedKey);
            var signedSignature = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var authorizationHeader = string.Format("{0} {1}:{2}", keyType, account, signedSignature);

            return authorizationHeader;
        }

        private static string GetResource(HttpRequestMessage request)
        {
            var resource = request.RequestUri.PathAndQuery;

            if (resource.Contains("?"))
            {
                resource = resource.Substring(0, resource.IndexOf("?", StringComparison.Ordinal));
            }

            return resource;
        }

        private static string GetStringToSign(HttpRequestMessage request, string account, KeyType keyType, string resource)
        {
            var headers = request.Headers;

            switch (keyType)
            {
                case KeyType.SharedKeyLite:
                    return string.Format("{0}\n/{1}{2}", headers.Date.GetValueOrDefault().ToString("R"), account, resource);

                case KeyType.SharedKey:
                    //return string.Format("{0}\n{1}\n{2}\n{3}\n/{4}/{5}",
                    //    request.Method,
                    //    TryGetHeaderValue(headers, "Content-MD5", ""),
                    //    TryGetHeaderValue(headers, "Content-Type", ""),
                    //    headers.Date.GetValueOrDefault().ToString("R"),
                    //    account,
                    //    resource);
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException(string.Format("KeyType.{0} has not been implemented.", keyType));

            }
        }

        private static string TryGetHeaderValue(HttpHeaders headers, string key, string defaultValue)
        {
            IEnumerable<string> values;

            return headers.TryGetValues(key, out values) ? values.Single() : defaultValue;
        }
    }
}