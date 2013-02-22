using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Tests.Mocks;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop.Tests
{
    [TestFixture]
    public class ProjectsViewModelTest
    {
        private IDataProvider dataProvider;

        [SetUp]
        public void SetUp()
        {
            dataProvider = new DataProvider(
                new MockedTeamCityClient(),
                new Worker { IsAsync = false });
        }

        [Test]
        public void ShouldHaveEmptyCollectionUntilExplicitlyLoaded()
        {
            var projects = new ProjectsViewModel(dataProvider);
            Assert.IsEmpty(projects.Collection);
            projects.LoadItems();
            Assert.IsNotEmpty(projects.Collection);
        }
    }
}
