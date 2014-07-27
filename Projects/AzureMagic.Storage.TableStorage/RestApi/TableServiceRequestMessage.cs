using System;
using System.Net.Http;
using AzureMagic.Storage.TableStorage.Helpers;
using AzureMagic.Storage.TableStorage.RestApi.PayloadFormats;

namespace AzureMagic.Storage.TableStorage.RestApi
{
    // todo: docs
    // todo: specs then turn public
    internal class TableServiceRequestMessage : HttpRequestMessage
    {
        internal TableServiceRequestMessage(HttpMethod method, StorageAccount account, string relativeUri, IPayloadFormat payloadFormat)
            : base(method, account.TablesUri.Append(relativeUri))
        {
            var headers = Headers;

            headers.Accept.Clear();
            headers.Accept.ParseAdd(payloadFormat.AcceptHeader);

            // Alternative header is x-ms-date.
            headers.Date = DateTime.UtcNow;

            // http://msdn.microsoft.com/en-us/library/azure/dd179405.aspx implies this value is optional.
            // However when accept header is application/json then x-ms-version header is required.
            headers.Add("x-ms-version", "2013-08-15");

            headers.Authorization = new TableServiceAuthenticationHeader(this, account);
        }
    }
}