using TechTalk.SpecFlow;

namespace AzureMagic.Storage.Tables.Specifications.Features.RESTAPI.Steps
{
    [Binding, Scope(Feature = "Tables")]
    public class TablesSteps
    {
        [Given(@"a valid connection string")]
        public void GivenAValidConnectionString()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"todo: write scenario")]
        public void GivenTodoWriteScenario()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"ListAsync\(\) is called")]
        public void WhenListAsyncIsCalled()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the connection's list of tables is returned")]
        public void ThenTheConnectionSListOfTablesIsReturned()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
