using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureMagic.Storage.TableStorage.RestApi
{
    public class Tables
    {
        private readonly string ConnectionString;

        public Tables(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public Task<IEnumerable<string>> ListAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
