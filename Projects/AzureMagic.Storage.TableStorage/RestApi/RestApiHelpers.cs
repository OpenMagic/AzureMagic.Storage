using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Anotar.CommonLogging;

namespace AzureMagic.Storage.TableStorage.RestApi
{
    public static class RestApiHelpers
    {
        public static string AuthorizationHeader(string method, DateTime now, HttpClient request, string relativeUri, string storageAccount, string storageKey, string ifMatch = "", string md5 = "", bool isTableStorage = true)
        {
            string messageSignature;

            if (isTableStorage)
            {
                var uri = new Uri(request.BaseAddress, relativeUri);

                LogTo.Debug("My: {0}", uri);

                messageSignature = String.Format("{0}\n\n{1}\n{2}\n{3}",
                    method,
                    request.DefaultRequestHeaders.Accept.Single().MediaType,
                    now.ToString("R", CultureInfo.InvariantCulture),
                    GetCanonicalizedResource(uri, storageAccount, isTableStorage)
                    );
            }
            else
            {
                throw new NotImplementedException();
                //messageSignature = String.Format("{0}\n\n\n{1}\n{5}\n\n\n\n{2}\n\n\n\n{3}{4}",
                //    method,
                //    (method == "GET" || method == "HEAD") ? String.Empty : request.ContentLength.ToString(),
                //    ifMatch,
                //    GetCanonicalizedHeaders(request),
                //    GetCanonicalizedResource(request.RequestUri, StorageAccount),
                //    md5
                //    );
            }
            var signatureBytes = Encoding.UTF8.GetBytes(messageSignature);
            var sha256 = new HMACSHA256(Convert.FromBase64String(storageKey));
            var authorizationHeader = "SharedKey " + storageAccount + ":" + Convert.ToBase64String(sha256.ComputeHash(signatureBytes));

            return authorizationHeader;
        }

        public static string GetCanonicalizedResource(Uri address, string accountName, bool isTableStorage)
        {
            var str = new StringBuilder();
            var builder = new StringBuilder("/");
            builder.Append(accountName);
            builder.Append(address.AbsolutePath);
            str.Append(builder);
            var values2 = new NameValueCollection();

            if (!isTableStorage)
            {
                throw new NotImplementedException();
                //NameValueCollection values = HttpUtility.ParseQueryString(address.Query);
                //foreach (string str2 in values.Keys)
                //{
                //    var list = new ArrayList(values.GetValues(str2));
                //    list.Sort();
                //    var builder2 = new StringBuilder();
                //    foreach (var obj2 in list)
                //    {
                //        if (builder2.Length > 0)
                //        {
                //            builder2.Append(",");
                //        }
                //        builder2.Append(obj2);
                //    }
                //    values2.Add((str2 == null) ? str2 : str2.ToLowerInvariant(), builder2.ToString());
                //}
            }

            var list2 = new ArrayList(values2.AllKeys);
            list2.Sort();
            foreach (string str3 in list2)
            {
                var builder3 = new StringBuilder(string.Empty);
                builder3.Append(str3);
                builder3.Append(":");
                builder3.Append(values2[str3]);
                str.Append("\n");
                str.Append(builder3);
            }
            return str.ToString();
        }
    }
}