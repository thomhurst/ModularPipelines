﻿using System.Text.Json;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

[Parallelizable(ParallelScope.Fixtures)]
public class ResultsRepositoryTests : TestBase
{
    public static readonly Folder Folder = Folder.CreateTemporaryFolder();

    private class JsonResultRepository : IModuleResultRepository
    {
        public async Task SaveResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
        {
            var file = Folder.CreateFile(module.GetType().FullName!);
            await using var fileStream = file.GetStream();
            await JsonSerializer.SerializeAsync(fileStream, moduleResult);
        }

        public async Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module, IPipelineHookContext pipelineContext)
        {
            var file = Folder.GetFile(module.GetType().FullName!);
            await using var fileStream = file.GetStream();
            return await JsonSerializer.DeserializeAsync<ModuleResult<T>>(fileStream);
        }
    }

    private class Module1 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    [DependsOn<Module1>]
    private class Module2 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    [Test, Order(1)]
    public async Task RunOne()
    {
        var pipeline = await TestPipelineHostBuilder.Create()
            .AddResultsRepository<JsonResultRepository>()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .ExecutePipelineAsync();

        Assert.That(pipeline.Modules.All(x => x.Status == Status.Successful), Is.True);
    }

    [Test, Order(2)]
    public async Task RunTwoFromHistory()
    {
        var pipeline = await TestPipelineHostBuilder.Create()
            .AddResultsRepository<JsonResultRepository>()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .RunCategories("Other")
            .ExecutePipelineAsync();

        Assert.That(pipeline.Modules.All(x => x.Status == Status.UsedHistory), Is.True);
    }
}