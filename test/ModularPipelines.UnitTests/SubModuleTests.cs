using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;

namespace ModularPipelines.UnitTests;

public class SubModuleTests : TestBase
{
    private class SubModulesModule : Module<string[]>
    {
        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .SelectAsync(name => SubModule(name, () =>
                {
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    return Task.FromResult(name);
                }))
                .ProcessInParallel();
        }
    }

    [Test]
    public async Task Submodule_Does_Not_Fail()
    {
        var module = await RunModule<SubModulesModule>();

        var results = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(results.ModuleResultType, Is.EqualTo(ModuleResultType.Success));
            Assert.That(results.Value, Is.EquivalentTo(new List<string> { "1", "2", "3" }));
        });
    }
}