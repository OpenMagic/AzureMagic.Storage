using System.Linq;
using AzureMagic.Storage.TableStorage.RestApi;
using AzureMagic.Storage.TableStorage.Specifications.Support.Contexts;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage;
using TechTalk.SpecFlow;

namespace AzureMagic.Storage.TableStorage.Specifications.Features.RestApi.Steps
{
    [Binding, Scope(Feature = "Tables")]
    public class TablesSteps
    {
        private readonly RestApiContext Context;
        private Tables Tables;
        private string[] TableNames;

        public TablesSteps(RestApiContext context)
        {
            Context = context;
        }

        [Given(@"Tables has been initialized")]
        public void GivenTablesHasBeenInitialized()
        {
            Tables = new Tables(Context.ConnectionString);
        }
        
        [Given(@"todo: write scenario")]
        public void GivenTodoWriteScenario()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"ListAsync\(\) is called")]
        public void WhenListAsyncIsCalled()
        {
            TableNames = Tables.ListAsync().Result.ToArray();
        }

        [Then(@"a list of tables for the current connection is returned")]
        public void ThenAListOfTablesForTheCurrentConnectionIsReturned()
        {
            var storageAccount = CloudStorageAccount.Parse(Context.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var tables = tableClient.ListTables();

            TableNames.ShouldAllBeEquivalentTo(tables.Select(t => t.Name));
        }
    }
}
