using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace AzureMagic.Storage.TableStorage.RestApi
{
    // todo: docs
    // todo: specs then turn public
    // todo: refactor
    internal class TableServiceAuthenticationHeader : AuthenticationHeaderValue
    {
        internal TableServiceAuthenticationHeader(HttpRequestMessage request, StorageAccount account)
            : base("SharedKey", CreateCredentials(request, account))
        {
        }

        private static string CreateCredentials(HttpRequestMessage request, StorageAccount account)
        {
            var signature = GetSignature(request, account.Name, account.Key);
            var credentials = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", account.Name, signature);

            return credentials;
        }

        private static string GetSignature(HttpRequestMessage request, string account, string storageKey)
        {
            var sharedKey = Convert.FromBase64String(storageKey);
            var resource = GetResource(request);
            var stringToSign = GetStringToSign(request, account, resource);
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

        private static string GetStringToSign(HttpRequestMessage request, string account, string resource)
        {
            var headers = request.Headers;

            return string.Format("{0}\n{1}\n{2}\n{3}\n/{4}{5}",
                request.Method,
                TryGetHeaderValue(headers, "Content-MD5", ""),
                TryGetHeaderValue(headers, "Content-Type", ""),
                request.Headers.Date.Value.ToString("R"),
                account,
                resource);
        }

        private static string TryGetHeaderValue(HttpHeaders headers, string key, string defaultValue)
        {
            IEnumerable<string> values;

            var value = headers.TryGetValues(key, out values) ? values.Single() : defaultValue;

            return value;
        }
    }
}