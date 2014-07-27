using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AzureMagic.Storage.TableStorage.Helpers;
using AzureMagic.Storage.TableStorage.RestApi;
using AzureMagic.Storage.TableStorage.Specifications.Support;
using AzureMagic.Storage.TableStorage.Specifications.Support.Contexts;
using Common.Logging;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace AzureMagic.Storage.TableStorage.Specifications.Features.RestApi.Steps
{
    [Binding, Scope(Feature = "TablesRestApi")]
    public class TablesRestApiSteps
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Context Context;
        private readonly RestApiContext RestApiContext;
        private HttpResponseMessage Response;

        private StorageAccount StorageAccount;
        private TablesRestApi TablesRestApi;

        public TablesRestApiSteps(Context context, RestApiContext restApiContext)
        {
            Context = context;
            RestApiContext = restApiContext;
        }

        [Given(@"TablesRestApi has been initialized")]
        public void GivenTablesHasBeenInitialized()
        {
            StorageAccount = StorageAccount.ForStorageEmulator();
            TablesRestApi = new TablesRestApi(StorageAccount);
        }

        [Given(@"todo: write scenario")]
        public void GivenTodoWriteScenario()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"ListAsync\(\) is called")]
        public void WhenListAsyncIsCalled()
        {
            Response = TablesRestApi.ListAsync().Result;
        }

        [Then(@"a successful HttpResponseMessage is returned")]
        public void ThenASuccessfulHttpResponseMessageIsReturned()
        {
            if (!Response.IsSuccessStatusCode)
            {
                Log.Warn(Response.ToLogFormat());
            }

            Response.IsSuccessStatusCode.Should().BeTrue("because the list tables request should be valid");
        }

        [Then(@"the HttpResponseMessage\.Content is a JSON list of tables for the current connection")]
        public void ThenTheHttpResponseMessage_ContentIsAJSONListOfTablesForTheCurrentConnection()
        {
            var expected = GetExpectedTableNames();
            var actual = GetActualTableNames();

            actual.ShouldBeEquivalentTo(expected);
        }

        private IEnumerable<string> GetActualTableNames()
        {
            var json = Response.Content.ReadAsAsync<JObject>().Result;
            var value = (JArray) json["value"];
            var tableNames = value.Select(jtoken => jtoken["TableName"].Value<string>());

            return tableNames;
        }

        private IEnumerable<string> GetExpectedTableNames()
        {
            var storageAccount = new CloudStorageAccount(new StorageCredentials(StorageAccount.Name, StorageAccount.Key), true);
            var tableClient = storageAccount.CreateCloudTableClient();
            var tables = tableClient.ListTables();

            return tables.Select(t => t.Name);
        }

        [Given(@"storageAccount is (.*)")]
        public void GivenStorageAccountIs(string storageAccount)
        {
            StorageAccount = storageAccount.FormatGivenValue(v => StorageAccount.ForStorageEmulator());
        }

        [When(@"Tables\(account\) is called")]
        public void WhenTablesAccountIsCalled()
        {
            try
            {
                TablesRestApi = new TablesRestApi(StorageAccount);
            }
            catch (Exception exception)
            {
                Context.Exception = exception;
            }
        }

        [Then(@"an instance is returned")]
        public void ThenAnInstanceIsReturned()
        {
            TablesRestApi.Should().NotBeNull();
        }
    }
}