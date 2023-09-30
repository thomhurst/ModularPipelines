using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using EnumerableAsyncProcessor.Extensions;

namespace ModularPipelines.UnitTests;

public class SubModuleTests : TestBase
{
    private class SubModulesWithReturnTypeModule : Module<string[]>
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

    private class SubModulesWithoutReturnTypeModule : Module<string[]>
    {
        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(name => SubModule(name, async () =>
                {
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    await Task.Yield();
                }))
                .ProcessInParallel();

            return await NothingAsync();
        }
    }

    [Test]
    public async Task Submodule_With_Return_Type_Does_Not_Fail()
    {
        var module = await RunModule<SubModulesWithReturnTypeModule>();

        var results = await module;

        Assert.Multiple(() =>
        {
            Assert.That(results.ModuleResultType, Is.EqualTo(ModuleResultType.Success));
            Assert.That(results.Value, Is.EquivalentTo(new List<string> { "1", "2", "3" }));
        });
    }
    
    [Test]
    public async Task Submodule_Without_Return_Type_Does_Not_Fail()
    {
        var module = await RunModule<SubModulesWithoutReturnTypeModule>();

        var results = await module;

        Assert.Multiple(() =>
        {
            Assert.That(results.ModuleResultType, Is.EqualTo(ModuleResultType.Success));
            Assert.That(results.Value, Is.Null);
        });
    }
}