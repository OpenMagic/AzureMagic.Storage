using System.Net.Http;
using System.Threading.Tasks;
using AzureMagic.Storage.TableStorage.RestApi.PayloadFormats;

namespace AzureMagic.Storage.TableStorage.RestApi
{
    // todo: docs
    public class TablesRestApi : TableServiceRestApi
    {
        public TablesRestApi(StorageAccount account)
            : base(account)
        {
        }

        public Task<HttpResponseMessage> ListAsync()
        {
            return ListAsync(new JsonFullMetadataPayloadFormat());
        }

        public Task<HttpResponseMessage> ListAsync(IPayloadFormat payloadFormat)
        {
            return Get("Tables", payloadFormat);
        }
    }
}