using System.Text.Json;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
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
        public async Task SaveResultAsync<T>(IModule module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
        {
            var file = Folder.CreateFile(module.GetType().FullName!);
            await using var fileStream = file.GetStream();
            await JsonSerializer.SerializeAsync(fileStream, moduleResult);
        }

        public async Task<ModuleResult<T>?> GetResultAsync<T>(IModule module, IPipelineHookContext pipelineContext)
        {
            var file = Folder.GetFile(module.GetType().FullName!);
            await using var fileStream = file.GetStream();
            return await JsonSerializer.DeserializeAsync<ModuleResult<T>>(fileStream);
        }
    }

    private class Module1 : Module
    {
        public override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<IDictionary<string, object>?>(null);
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : Module
    {
        public override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<IDictionary<string, object>?>(null);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ResultsRepositoryTests), Order = 1)]
    public async Task RunOne()
    {
        var pipeline = await TestPipelineHostBuilder.Create()
            .AddResultsRepository<JsonResultRepository>()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .ExecutePipelineAsync();
        await Assert.That(pipeline.Modules.All(x => x.Status == Status.Successful)).IsTrue();
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ResultsRepositoryTests), Order = 2)]
    public async Task RunTwoFromHistory()
    {
        var pipeline = await TestPipelineHostBuilder.Create()
            .AddResultsRepository<JsonResultRepository>()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .RunCategories("Other")
            .ExecutePipelineAsync();
        await Assert.That(pipeline.Modules.All(x => x.Status == Status.UsedHistory)).IsTrue();
    }
}