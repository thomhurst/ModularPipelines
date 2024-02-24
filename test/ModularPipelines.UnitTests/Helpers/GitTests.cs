using ModularPipelines.Context;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Helpers;

public class GitTests : TestBase
{
    private class GitVersionModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.Git().Commands.Git(new GitBaseOptions
            {
                Version = true,
            }, token: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<GitVersionModule>();

        var moduleResult = await module;
        
        await Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(moduleResult.Exception).Is.Null();
            Assert.That(moduleResult.Value).Is.Not.Null();
        });
    }

    [Test]
    public async Task Standard_Output_Starts_With_Git_Version()
    {
        var module = await RunModule<GitVersionModule>();

        var moduleResult = await module;
        
        await Assert.Multiple(() =>
        {
            Assert.That(moduleResult.Value!.StandardError).Is.Null().Or.Is.Empty();
            Assert.That(moduleResult.Value.StandardOutput).Does.Match("git version \\d+.*");
        });
    }

    [Test]
    public async Task GitRootDirectory()
    {
        var git = await GetService<IGit>();
        
        await Assert.Multiple(() =>
        {
            Assert.That(git.RootDirectory.Name).Is.EqualTo("ModularPipelines");
            Assert.That(git.RootDirectory.ListFiles().Select(x => x.Name)).Does.Contain("README.md");
        });
    }

    [Test]
    public async Task DefaultBranchName()
    {
        var git = await GetService<IGit>();
        await Assert.That(git.Information.DefaultBranchName).Is.EqualTo("main");
    }
}