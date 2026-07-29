using ModularPipelines.Context;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

public class GitTests : TestBase
{
    private class GitVersionModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Git().Commands.Repository.GitAsync(new GitBaseOptions
            {
                Version = true,
            }, cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<GitVersionModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    public async Task Standard_Output_Starts_With_Git_Version()
    {
        var moduleResult = await await RunModule<GitVersionModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ValueOrDefault!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.ValueOrDefault.StandardOutput).Matches(@"git version \d+.*");
        }
    }

    [Test]
    public async Task GitRepositoryInfo()
    {
        var git = await GetService<IGit>();
        var repositoryInfo = await git.Information.GetInfoAsync();

        using (Assert.Multiple())
        {
            await Assert.That(repositoryInfo).IsNotNull();
            await Assert.That(repositoryInfo!.Root.ListFiles().Select(x => x.Name)).Contains("README.md");
        }
    }

    [Test]
    public async Task DefaultBranchName()
    {
        var git = await GetService<IGit>();
        var repositoryInfo = await git.Information.GetInfoAsync();
        await Assert.That(repositoryInfo?.DefaultBranchName).IsEqualTo("main");
    }

    [Test]
    public async Task Commits_Are_Available_Through_Interface()
    {
        var git = await GetService<IGit>();
        await using var commits = git.Information.Commits().GetAsyncEnumerator();

        await Assert.That(await commits.MoveNextAsync()).IsTrue();
    }

    [Test]
    public async Task Commands_Are_Grouped_And_Asynchronous()
    {
        var groups = typeof(IGitCommands).GetProperties();
        var commandMethods = groups
            .SelectMany(property => property.PropertyType.GetMethods())
            .Where(method => !method.IsSpecialName)
            .ToList();

        using (Assert.Multiple())
        {
            await Assert.That(groups).Count().IsEqualTo(6);
            await Assert.That(typeof(IGitCommands).GetMethods().All(method => method.IsSpecialName)).IsTrue();
            await Assert.That(commandMethods).Count().IsEqualTo(80);
            await Assert.That(commandMethods.All(method => method.Name.EndsWith("Async", StringComparison.Ordinal))).IsTrue();
        }
    }
}
