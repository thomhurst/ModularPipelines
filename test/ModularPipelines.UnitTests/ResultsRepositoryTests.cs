using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class ResultsRepositoryTests : TestBase
{
    public static readonly Folder Folder = Folder.CreateTemporaryFolder();

    private class JsonResultRepository : IModuleResultRepository
    {
        public async Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
        {
            var file = Folder.CreateFile(module.GetType().FullName!);
            await using var fileStream = file.GetStream();
            await JsonSerializer.SerializeAsync(fileStream, moduleResult);
        }

        public async Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineHookContext pipelineContext)
        {
            var file = Folder.GetFile(module.GetType().FullName!);
            await using var fileStream = file.GetStream();
            return await JsonSerializer.DeserializeAsync<ModuleResult<T>>(fileStream);
        }
    }

    private class Module1 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ResultsRepositoryTests), Order = 1)]
    public async Task RunOne()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddResultsRepository<JsonResultRepository>()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var module1Result = resultRegistry.GetResult(typeof(Module1))!;
        var module2Result = resultRegistry.GetResult(typeof(Module2))!;

        using (Assert.Multiple())
        {
            await Assert.That(module1Result.ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(module2Result.ModuleStatus).IsEqualTo(Status.Successful);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ResultsRepositoryTests), Order = 2)]
    public async Task RunTwoFromHistory()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddResultsRepository<JsonResultRepository>()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .RunCategories("Other")
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var module1Result = resultRegistry.GetResult(typeof(Module1))!;
        var module2Result = resultRegistry.GetResult(typeof(Module2))!;

        using (Assert.Multiple())
        {
            await Assert.That(module1Result.ModuleStatus).IsEqualTo(Status.UsedHistory);
            await Assert.That(module2Result.ModuleStatus).IsEqualTo(Status.UsedHistory);
        }
    }
}
