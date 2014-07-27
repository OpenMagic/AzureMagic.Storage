using System;

namespace AzureMagic.Storage.TableStorage.Specifications.Support.Contexts
{
    public class Context
    {
        public Exception Exception;

        public ArgumentNullException ArgumentNullException
        {
            get { return Exception as ArgumentNullException; }
        }

        public ArgumentException ArgumentException
        {
            get { return Exception as ArgumentException; }
        }
    }
}