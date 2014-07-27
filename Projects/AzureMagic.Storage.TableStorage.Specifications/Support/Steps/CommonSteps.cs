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

        [Then(@"ArgumentNullException is thrown for (.*)")]
        public void ThenArgumentNullExceptionIsThrownFor(string paramName)
        {
            Context.ArgumentNullException.Should().NotBeNull();
            Context.ArgumentNullException.ParamName.Should().Be(paramName);
        }

        [Then(@"ArgumentException is thrown for (.*)")]
        public void ThenArgumentExceptionIsThrownFor(string paramName)
        {
            Context.ArgumentException.Should().NotBeNull();
            Context.ArgumentException.ParamName.Should().Be(paramName);
        }
    }
}