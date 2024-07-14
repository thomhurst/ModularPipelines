using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.GitHub;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions;

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
            await Assert.That(gitHubRepositoryInfo).Is.Not.Null();
            await Assert.That(gitHubRepositoryInfo.IsInitialized).Is.True();
            await Assert.That(gitHubRepositoryInfo.IsGitHub).Is.True();
            await Assert.That(gitHubRepositoryInfo.RepositoryName).Is.Not.Null()
                .And.Is.Not.Empty();
            await Assert.That(gitHubRepositoryInfo.Owner).Is.Not.Null()
                .And.Is.Not.Empty();
            await Assert.That(gitHubRepositoryInfo.Endpoint).Is.Not.Null()
                .And.Is.Not.Empty();
            await Assert.That(gitHubRepositoryInfo.Identifier).Is.Not.Null()
                .And.Is.Not.Empty();
        }
    }
}