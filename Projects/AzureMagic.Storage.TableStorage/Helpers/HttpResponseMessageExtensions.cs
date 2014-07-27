using System.Net.Http;
using AzureMagic.Storage.TableStorage.Helpers.Yaml;

namespace AzureMagic.Storage.TableStorage.Helpers
{
    public static class HttpResponseMessageExtensions
    {
        public static string ToLogFormat(this HttpResponseMessage response)
        {
            var value = new
            {
                Status = string.Format("{0}. {1}", response.StatusCode, response.ReasonPhrase),
                response
            };

            return YamlConverter.Serialize(value);
        }
    }
}