namespace AzureMagic.Storage.TableStorage.RestApi.PayloadFormats
{
    // todo: docs
    public abstract class PayloadFormat : IPayloadFormat
    {
        // todo: specs
        // todo: docs
        protected PayloadFormat(string acceptHeader)
        {
            AcceptHeader = acceptHeader;
        }

        // todo: docs
        public string AcceptHeader { get; private set; }
    }
}