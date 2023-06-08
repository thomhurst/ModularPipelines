using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Extensions;

namespace ModularPipelines.UnitTests.Helpers;

public class NodeTests : TestBase
{
    private class NodeVersionModule : Module<BufferedCommandResult>
    {
        protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var task = () =>
            {
                context.Node().Npm.Install(null!, cancellationToken);
                context.Node().Npm.Run(null!, cancellationToken);
                context.Node().Nvm.Use(null!, cancellationToken);
            };
            
            return await context.Node().Version(cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<NodeVersionModule>();

        var moduleResult = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.SuccessfulResult));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task Standard_Output_Is_Version_Number()
    {
        var module = await RunModule<NodeVersionModule>();

        var moduleResult = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.Value!.StandardError, Is.Null.Or.Empty);
            Assert.That(moduleResult.Value.StandardOutput, Does.Match("v\\d+"));
        });
    }
}