using System;
using System.Linq;
using AzureMagic.Storage.TableStorage.RestApi;
using AzureMagic.Storage.TableStorage.Specifications.Support;
using AzureMagic.Storage.TableStorage.Specifications.Support.Contexts;
using AzureMagic.Storage.TableStorage.Specifications.Support.Steps;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage;
using TechTalk.SpecFlow;

namespace AzureMagic.Storage.TableStorage.Specifications.Features.RestApi.Steps
{
    [Binding, Scope(Feature = "Tables")]
    public class TablesSteps
    {
        private readonly Context Context;
        private readonly RestApiContext RestApiContext;

        private string ConnectionString;
        private string[] TableNames;
        private Tables Tables;

        public TablesSteps(Context context, RestApiContext restApiContext)
        {
            Context = context;
            RestApiContext = restApiContext;
        }

        [Given(@"Tables has been initialized")]
        public void GivenTablesHasBeenInitialized()
        {
            Tables = new Tables(RestApiContext.ConnectionString);
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
            var storageAccount = CloudStorageAccount.Parse(RestApiContext.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var tables = tableClient.ListTables();

            TableNames.ShouldAllBeEquivalentTo(tables.Select(t => t.Name));
        }

        [Given(@"connectionString is (.*)")]
        public void GivenConnectionStringIs(string connectionString)
        {
            ConnectionString = connectionString.FormatGivenValue();

            if (ConnectionString == "valid")
            {
                ConnectionString = RestApiContext.ConnectionString;
            }
        }

        [When(@"Tables\(connectionString\) is called")]
        public void WhenTablesConnectionStringIsCalled()
        {
            try
            {
                Tables = new Tables(ConnectionString);
            }
            catch (Exception exception)
            {
                Context.Exception = exception;
            }
        }

        [Then(@"an instance is returned")]
        public void ThenAnInstanceIsReturned()
        {
            Tables.Should().NotBeNull();
        }
    }
}