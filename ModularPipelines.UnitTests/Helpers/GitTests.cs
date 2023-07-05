using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Helpers;

public class GitTests : TestBase
{
    private class GitVersionModule : Module<CommandResult>
    {
        protected override async Task<ModuleResult<CommandResult>?> ExecuteAsync( IModuleContext context, CancellationToken cancellationToken )
        {
            return await context.Git().Operations.Version( cancellationToken: cancellationToken );
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<GitVersionModule>();

        var moduleResult = await module;

        Assert.Multiple( () =>
        {
            Assert.That( moduleResult.ModuleResultType, Is.EqualTo( ModuleResultType.SuccessfulResult ) );
            Assert.That( moduleResult.Exception, Is.Null );
            Assert.That( moduleResult.Value, Is.Not.Null );
        } );
    }

    [Test]
    public async Task Standard_Output_Starts_With_Git_Version()
    {
        var module = await RunModule<GitVersionModule>();

        var moduleResult = await module;

        Assert.Multiple( () =>
        {
            Assert.That( moduleResult.Value!.StandardError, Is.Null.Or.Empty );
            Assert.That( moduleResult.Value.StandardOutput, Does.Match( "git version \\d+.*" ) );
        } );
    }
}
