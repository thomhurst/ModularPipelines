using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
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
    public interface IProcessor<T>;

    public class InterfaceCacheModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithCacheKeyPart("interface-cache");

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(GetType().GetInterfaces().Single(contract =>
                contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(IProcessor<>))
                .GetGenericArguments()[0].Module.ModuleVersionId.ToString());
    }

    public class VersionOverrideInterfaceCacheModule : InterfaceCacheModule
    {
        protected override void Configure(ModuleConfigurationBuilder module)
        {
            base.Configure(module);
            module.WithCacheAssemblyVersionKey("unchanged-interface-module");
        }
    }

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

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Changed_Interface_Argument_Build_Invalidates_Cache(bool assemblyOverride, bool inherited)
    {
        var assemblyName = $"InterfaceCacheModel_{Guid.NewGuid():N}";
        var firstModel = CreateModel(assemblyName, 1);
        var secondModel = CreateModel(assemblyName, 2);
        var module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName($"InterfaceCache_{Guid.NewGuid():N}"), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("InterfaceCache");
        var parent = assemblyOverride ? typeof(VersionOverrideInterfaceCacheModule) : typeof(InterfaceCacheModule);
        var firstModule = CreateInterfaceModule(module, "First", parent, firstModel, inherited);
        var secondModule = CreateInterfaceModule(module, "Second", parent, secondModel, inherited);
        await Assert.That(firstModule.IsGenericType).IsFalse();
        await Assert.That(ModuleId.FromType(firstModule)).IsEqualTo(ModuleId.FromType(secondModule));
        await Assert.That(firstModule.Module.ModuleVersionId).IsEqualTo(secondModule.Module.ModuleVersionId);
        await Assert.That(secondModule.GetInterfaces()).Contains(typeof(IProcessor<>).MakeGenericType(secondModel));

        var directory = Directory.CreateTempSubdirectory("ModularPipelines-interface-cache-");
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

    private static Type CreateInterfaceModule(ModuleBuilder module, string name, Type parent, Type model, bool inherited)
    {
        var builder = module.DefineType(name, TypeAttributes.Public, parent);
        builder.AddInterfaceImplementation(typeof(IProcessor<>).MakeGenericType(model));
        if (inherited)
        {
            builder = module.DefineType($"{name}Derived", TypeAttributes.Public, builder.CreateType()!);
        }

        builder.SetCustomAttribute(new CustomAttributeBuilder(typeof(ModuleIdAttribute).GetConstructor([typeof(string)])!, ["interface-cache"]));
        return builder.CreateType()!;
    }

    private static Type CreateModel(string assemblyName, int version = 1) => AssemblyBuilder
        .DefineDynamicAssembly(new AssemblyName(assemblyName) { Version = new Version(version, 0, 0, 0) }, AssemblyBuilderAccess.RunAndCollect)
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
