using System.Net.Http;
using System.Threading.Tasks;
using AzureMagic.Storage.TableStorage.RestApi.PayloadFormats;

namespace AzureMagic.Storage.TableStorage.RestApi
{
    // todo: docs
    // todo: specs then turn public
    public class TableServiceRestApi
    {
        private readonly StorageAccount Account;

        internal TableServiceRestApi(StorageAccount account)
        {
            Account = account;
        }

        internal async Task<HttpResponseMessage> Get(string resource, IPayloadFormat payloadFormat)
        {
            using (var client = new HttpClient())
            using (var request = new TableServiceRequestMessage(HttpMethod.Get, Account, resource, payloadFormat))
            {
                return await client.SendAsync(request);
            }
        }
    }
}