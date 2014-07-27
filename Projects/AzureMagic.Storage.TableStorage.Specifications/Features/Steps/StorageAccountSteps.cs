using System;
using AzureMagic.Storage.TableStorage.Specifications.Support;
using AzureMagic.Storage.TableStorage.Specifications.Support.Contexts;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AzureMagic.Storage.TableStorage.Specifications.Features.Steps
{
    [Binding, Scope(Feature = "StorageAccount")]
    public class StorageAccountSteps
    {
        private readonly Context Context;
        private string AccountKey;
        private string AccountName;
        private Uri TablesUri;
        private StorageAccount StorageAccount;

        public StorageAccountSteps(Context context)
        {
            Context = context;
        }

        [Given(@"accountName is (.*)")]
        public void GivenAccountNameIs(string accountName)
        {
            AccountName = accountName.FormatGivenValue();
        }

        [Given(@"accountKey is (.*)")]
        public void GivenAccountKeyIs(string accountKey)
        {
            AccountKey = accountKey.FormatGivenValue();
        }

        [Given(@"tablesUri is (.*)")]
        public void GivenTablesUriIs(string uri)
        {
            TablesUri = uri.FormatGivenValue(u => new Uri(u));
        }

        [When(@"Constructor\(accountName, accountKey\) is called")]
        public void WhenConstructorAccountNameAccountKeyIsCalled()
        {
            try
            {
                StorageAccount = new StorageAccount(AccountName, AccountKey);
            }
            catch (Exception exception)
            {
                Context.Exception = exception;
            }
        }
        
        [When(@"Constructor\(accountName, accountKey, tablesUri\) is called")]
        public void WhenConstructorAccountNameAccountKeyTablesUriIsCalled()
        {
            try
            {
                StorageAccount = new StorageAccount(AccountName, AccountKey, TablesUri);
            }
            catch (Exception exception)
            {
                Context.Exception = exception;
            }
        }

        [When(@"ForStorageEmulator\(\) is called")]
        public void WhenForStorageEmulatorIsCalled()
        {
            StorageAccount = StorageAccount.ForStorageEmulator();
        }

        [Then(@"a StorageAccount instance is returned")]
        public void ThenAStorageAccountInstanceIsReturned()
        {
            StorageAccount.Should().NotBeNull();
        }

        [Then(@"Name should be (.*)")]
        public void ThenNameShouldBe(string expectedName)
        {
            if (expectedName == "accountName")
            {
                expectedName = AccountName;
            }
            StorageAccount.Name.Should().Be(expectedName);
        }

        [Then(@"Key should be (.*)")]
        public void ThenKeyShouldBe(string expectedKey)
        {
            if (expectedKey == "accountKey")
            {
                expectedKey = AccountKey;
            }
            StorageAccount.Key.Should().Be(expectedKey);
        }

        [Then(@"TablesUri should be (.*)")]
        public void ThenTablesUriShouldBe(string expectedUri)
        {
            if (expectedUri == "tablesUri")
            {
                expectedUri = TablesUri.ToString();
            }

            StorageAccount.TablesUri.ToString().Should().Be(expectedUri);
        }

        [When(@"CreateTablesUri\(accountName\) is called")]
        public void WhenCreateTablesUriAccountNameIsCalled()
        {
            try
            {
                TablesUri = StorageAccount.CreateTablesUri(AccountName);
            }
            catch (Exception exception)
            {
                Context.Exception = exception;
            }
        }

        [Then(@"Uri (.*) should be returned")]
        public void ThenUriShouldBeReturned(string expectedUri)
        {
            TablesUri.ToString().Should().Be(expectedUri);
        }
    }
}