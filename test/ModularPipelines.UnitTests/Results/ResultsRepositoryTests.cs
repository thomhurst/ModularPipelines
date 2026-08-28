using ModularPipelines.Reporting;
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
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Results;

public class ResultsRepositoryTests : TestBase
{
    public static readonly FolderPath Folder = FolderPath.CreateTemporaryFolder();

    private class JsonResultRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public async Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext)
        {
            var file = Folder.CreateFile(module.GetType().FullName!);
            await using var fileStream = file.GetStream();
            await JsonSerializer.SerializeAsync(fileStream, moduleResult);
        }

        public async Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineContext pipelineContext)
        {
            var file = Folder.GetFile(module.GetType().FullName!);
            await using var fileStream = file.GetStream();
            return await JsonSerializer.DeserializeAsync<ModuleResult<T>>(fileStream);
        }
    }

    private class Module1 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.DependsOn<Module1>]
    private class Module2 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModuleCategory("Other")]
    private class OtherCategoryModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ResultsRepositoryTests), Order = 1)]
    public async Task RunOne()
    {
        var host = await TestPipelineBuilder.Create()
            .AddResultsRepository<JsonResultRepository>()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var module1Result = resultRegistry.GetResult(typeof(Module1))!;
        var module2Result = resultRegistry.GetResult(typeof(Module2))!;

        using (Assert.Multiple())
        {
            await Assert.That(module1Result.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(module2Result.Status).IsEqualTo(ModuleStatus.Succeeded);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ResultsRepositoryTests), Order = 2)]
    public async Task RunTwoFromHistory()
    {
        var host = await TestPipelineBuilder.Create()
            .AddResultsRepository<JsonResultRepository>()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<OtherCategoryModule>()
            .ConfigureOptions(options => options with { RunOnlyCategories = ["Other"] })
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var module1Result = resultRegistry.GetResult(typeof(Module1))!;
        var module2Result = resultRegistry.GetResult(typeof(Module2))!;

        using (Assert.Multiple())
        {
            await Assert.That(module1Result.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
            await Assert.That(module2Result.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
        }
    }
}
