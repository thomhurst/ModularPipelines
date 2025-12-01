using ModularPipelines.Context;
using ModularPipelines.GitHub;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class GitHubRepositoryInfoTests : TestBase
{
    public class GitRepoModule : IModule<IGitHubRepositoryInfo>
    {
        public async Task<IGitHubRepositoryInfo?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.GitHub().RepositoryInfo;
        }
    }

    [Test]
    public async Task GitHub_Repository_Information_Is_Populated()
    {
        var moduleResult = await RunModuleWithResult<GitRepoModule, IGitHubRepositoryInfo>();

        var gitHubRepositoryInfo = moduleResult.Value!;

        using (Assert.Multiple())
        {
            await Assert.That(gitHubRepositoryInfo).IsNotNull();
            await Assert.That(gitHubRepositoryInfo.IsInitialized).IsTrue();
            await Assert.That(gitHubRepositoryInfo.IsGitHub).IsTrue();
            await Assert.That(gitHubRepositoryInfo.RepositoryName).IsNotNull()
                .And.IsNotEmpty();
            await Assert.That(gitHubRepositoryInfo.Owner).IsNotNull()
                .And.IsNotEmpty();
            await Assert.That(gitHubRepositoryInfo.Endpoint).IsNotNull()
                .And.IsNotEmpty();
            await Assert.That(gitHubRepositoryInfo.Identifier).IsNotNull()
                .And.IsNotEmpty();
        }
    }
}
