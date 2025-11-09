using ModularPipelines.Context;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class GitTests : TestBase
{
    private class GitVersionModule : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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

        var moduleResult = (ModuleResult<CommandResult>)await module.GetModuleResult();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task Standard_Output_Starts_With_Git_Version()
    {
        var module = await RunModule<GitVersionModule>();

        var moduleResult = (ModuleResult<CommandResult>)await module.GetModuleResult();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.Value.StandardOutput).Matches("git version \\d+.*");
        }
    }

    [Test]
    public async Task GitRootDirectory()
    {
        var git = await GetService<IGit>();

        using (Assert.Multiple())
        {
            await Assert.That(git.RootDirectory.Name).IsEqualTo("ModularPipelines");
            await Assert.That(git.RootDirectory.ListFiles().Select(x => x.Name)).Contains("README.md");
        }
    }

    [Test]
    public async Task DefaultBranchName()
    {
        var git = await GetService<IGit>();
        await Assert.That(git.Information.DefaultBranchName).IsEqualTo("main");
    }
}