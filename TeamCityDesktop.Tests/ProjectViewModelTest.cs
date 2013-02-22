using NUnit.Framework;

using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Tests.Mocks;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop.Tests
{
    [TestFixture]
    public class ProjectViewModelTest
    {
        private IDataProvider dataProvider;
        private ProjectsViewModel projectsViewModel;

        [SetUp]
        public void SetUp()
        {
            dataProvider = new DataProvider(
                new MockedTeamCityClient(),
                new Worker { IsAsync = false });
            projectsViewModel = new ProjectsViewModel(dataProvider);
            projectsViewModel.LoadItems();
        }

        [Test]
        public void ShouldLoadItemsWhenExpanded()
        {
            ProjectViewModel project = projectsViewModel.Collection[0];
            Assert.IsEmpty(project.Collection);
            project.IsExpanded = true;
            Assert.IsNotEmpty(project.Collection);
        }
    }
}
