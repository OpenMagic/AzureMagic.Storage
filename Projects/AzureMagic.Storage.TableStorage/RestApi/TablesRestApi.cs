using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Anotar.CommonLogging;

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
                client.BaseAddress = Account.TablesUri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json;odata=fullmetadata");

                foreach (var mediaTypeWithQualityHeaderValue in client.DefaultRequestHeaders.Accept)
                {
                    LogTo.Debug(mediaTypeWithQualityHeaderValue.ToString());
                }

                client.DefaultRequestHeaders.Add("x-ms-date", now);

                // http://msdn.microsoft.com/en-us/library/azure/dd179405.aspx implies this value is optional.
                // However when accept header is application/json then x-ms-version header is required.
                client.DefaultRequestHeaders.Add("x-ms-version", "2013-08-15");

                client.DefaultRequestHeaders.Add("Authorization", GetAuthorizationHeader(Account.Name, Account.Key, now, new Uri(Account.TablesUri, "Tables")));

                return await client.GetAsync("Tables");
            }
        }

        private static string GetAuthorizationHeader(string account, string storageKey, string now, Uri uri)
        {
            var sharedKey = Convert.FromBase64String(storageKey);
            var resource = uri.PathAndQuery;

            if (resource.Contains("?"))
            {
                resource = resource.Substring(0, resource.IndexOf("?", StringComparison.Ordinal));
            }

            var stringToSign = string.Format("{0}\n/{1}{2}", now, account, resource);
            var hasher = new HMACSHA256(sharedKey);
            var signedSignature = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var authorizationHeader = string.Format("{0} {1}:{2}", "SharedKeyLite", account, signedSignature);

            return authorizationHeader;
        }
    }
}