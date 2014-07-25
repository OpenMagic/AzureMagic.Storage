using System;
using AzureMagic.Storage.TableStorage.Specifications.Support.Contexts;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AzureMagic.Storage.TableStorage.Specifications.Support.Steps
{
    [Binding]
    public class CommonSteps
    {
        private readonly Context Context;

        public CommonSteps(Context context)
        {
            Context = context;
        }

        [Then(@"ArgumentNullException is thrown")]
        public void ThenArgumentNullExceptionIsThrown()
        {
            Context.Exception.Should().BeOfType<ArgumentNullException>();
        }

        [Then(@"ArgumentException is thrown")]
        public void ThenArgumentExceptionIsThrown()
        {
            Context.Exception.Should().BeOfType<ArgumentException>();
        }
    }
}