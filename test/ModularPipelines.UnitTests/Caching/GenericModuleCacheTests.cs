using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Caching;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Caching;

public class GenericModuleCacheTests
{
    public class GenericCacheModule<T> : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithCacheKeyPart("generic-cache");

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(typeof(T).Module.ModuleVersionId.ToString());
    }

    public class VersionOverrideCacheModule<T> : GenericCacheModule<T>
    {
        protected override void Configure(ModuleConfigurationBuilder module)
        {
            base.Configure(module);
            module.WithCacheAssemblyVersionKey("unchanged-module");
        }
    }

    [Test]
    [Arguments(typeof(GenericCacheModule<>))]
    [Arguments(typeof(VersionOverrideCacheModule<>))]
    public async Task Changed_Generic_Argument_Build_Invalidates_Cache(Type moduleDefinition)
    {
        var assemblyName = $"CacheModel_{Guid.NewGuid():N}";
        var firstModel = CreateModel(assemblyName);
        var secondModel = CreateModel(assemblyName);
        var firstModule = moduleDefinition.MakeGenericType(firstModel);
        var secondModule = moduleDefinition.MakeGenericType(secondModel);
        await Assert.That(ModuleId.FromType(firstModule)).IsEqualTo(ModuleId.FromType(secondModule));
        await Assert.That(firstModule.Module.ModuleVersionId).IsEqualTo(secondModule.Module.ModuleVersionId);
        await Assert.That(firstModel.Module.ModuleVersionId).IsNotEqualTo(secondModel.Module.ModuleVersionId);

        var directory = Directory.CreateTempSubdirectory("ModularPipelines-generic-cache-");
        try
        {
            var first = await RunAsync(firstModule, directory.FullName);
            var repeated = await RunAsync(firstModule, directory.FullName);
            var changed = await RunAsync(secondModule, directory.FullName);
            await Assert.That(first.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(repeated.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
            await Assert.That(changed.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(changed.ValueOrDefault).IsEqualTo(secondModel.Module.ModuleVersionId.ToString());
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    private static Type CreateModel(string assemblyName) => AssemblyBuilder
        .DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndCollect)
        .DefineDynamicModule(assemblyName)
        .DefineType("ExternalModel", TypeAttributes.Public)
        .CreateType()!;

    private static async Task<IModuleResult> RunAsync(Type moduleType, string directory)
    {
        var builder = TestPipelineBuilder.Create()
            .AddModules(moduleType)
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = directory;
                options.CacheDirectory = Path.Combine(directory, "cache");
            });
        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync();
        return pipeline.Services.GetRequiredService<IModuleResultRegistry>().GetResult(moduleType)!;
    }
}
