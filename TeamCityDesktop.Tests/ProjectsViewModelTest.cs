using NUnit.Framework;

using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Tests.Mocks;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop.Tests
{
    [TestFixture]
    public class ProjectsViewModelTest
    {
        private readonly MockedTeamCityClient mockedClient =
            new MockedTeamCityClient();
        private IDataProvider dataProvider;

        [SetUp]
        public void SetUp()
        {
            dataProvider = new DataProvider(
                mockedClient,
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

        [Test]
        public void ShouldPropagateSelectedBuild()
        {
            var projects = new ProjectsViewModel(dataProvider);
            projects.LoadItems();
            ProjectViewModel projectViewModel = projects.Collection[0];
            projectViewModel.LoadItems();
            BuildConfigViewModel buildConfigViewModel = projectViewModel.Collection[0];
            buildConfigViewModel.LoadItems();
            BuildViewModel buildViewModel = buildConfigViewModel.Collection[0];
            buildConfigViewModel.SelectedItem = buildViewModel;
            Assert.AreSame(buildViewModel, projects.SelectedBuild);
        }
    }
}
