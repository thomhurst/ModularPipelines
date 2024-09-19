using ModularPipelines.Context;
using ModularPipelines.GitHub;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class GitHubRepositoryInfoTests : TestBase
{
    public class GitRepoModule : Module<IGitHubRepositoryInfo>
    {
        protected override async Task<IGitHubRepositoryInfo?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.GitHub().RepositoryInfo;
        }
    }
    
    [Test]
    public async Task GitHub_Repository_Information_Is_Populated()
    {
        var gitRepoModule = await RunModule<GitRepoModule>();

        var gitHubRepositoryInfo = gitRepoModule.Result.Value!;
        
        await using (Assert.Multiple())
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