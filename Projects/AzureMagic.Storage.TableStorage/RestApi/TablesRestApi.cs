using System;
using System.Globalization;
using System.Net.Http;
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

        private static string GetAuthorizationHeader(HttpRequestMessage request, string account, string storageKey)
        {
            var sharedKey = Convert.FromBase64String(storageKey);
            var resource = request.RequestUri.PathAndQuery;

            if (resource.Contains("?"))
            {
                resource = resource.Substring(0, resource.IndexOf("?", StringComparison.Ordinal));
            }

            var stringToSign = string.Format("{0}\n/{1}{2}", request.Headers.Date.Value.ToString("R"), account, resource);
//            var stringToSign = string.Format("{0}\n{1}\n{2}\n{3}\n/{4}/{5}",
//method,
//    client.DefaultRequestHeaders["Content-MD5"],
//    request.Headers["Content-Type"],
//    request.Headers["x-ms-date"],
//    account,
//    resource);

            var hasher = new HMACSHA256(sharedKey);
            var signedSignature = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var authorizationHeader = string.Format("{0} {1}:{2}", "SharedKeyLite", account, signedSignature);

            return authorizationHeader;
        }
    }
}