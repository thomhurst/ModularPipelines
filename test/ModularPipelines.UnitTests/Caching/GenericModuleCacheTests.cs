using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    public class PluginCacheModule(object value) : Module<object>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithCacheKeyPart("plugin-cache");

        protected internal override Task<object> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult(value);
    }

    [Test]
    public async Task Cached_Plugin_Value_Resolves_Outside_Module_Context()
    {
        using var builds = new ModuleLoadContextBuilds(typeof(Module<object>), identicalBuilds: false);
        var value = Activator.CreateInstance(builds.Second.GetType("RuntimeValue")!)!;
        var directory = Directory.CreateTempSubdirectory("ModularPipelines-plugin-cache-");
        try
        {
            await RunAsync(typeof(PluginCacheModule), directory.FullName, value);
            var repeated = await RunAsync(typeof(PluginCacheModule), directory.FullName, value);
            await Assert.That(repeated.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
            await Assert.That(repeated.ValueOrDefault!.GetType()).IsEqualTo(value.GetType());
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    public abstract class BaseBehaviorCacheModule : Module<string>
    {
        public abstract string GetLabel();

        protected override void Configure(ModuleConfigurationBuilder module) => module.WithCacheKeyPart("base-behavior-cache");

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult(GetLabel());
    }

    public abstract class VersionOverrideBaseBehaviorCacheModule : BaseBehaviorCacheModule
    {
        protected override void Configure(ModuleConfigurationBuilder module)
        {
            base.Configure(module);
            module.WithCacheAssemblyVersionKey("unchanged-leaf-module");
        }
    }

    [Test]
    [Arguments(typeof(BaseBehaviorCacheModule))]
    [Arguments(typeof(VersionOverrideBaseBehaviorCacheModule))]
    public async Task Changed_Base_Module_Build_Invalidates_Cache(Type moduleBase)
    {
        using var builds = new ModuleBaseBuilds(moduleBase);
        await Assert.That(builds.First.Module.ModuleVersionId).IsEqualTo(builds.Second.Module.ModuleVersionId);
        var directory = Directory.CreateTempSubdirectory("ModularPipelines-base-module-cache-");
        try
        {
            var first = await RunAsync(builds.First, directory.FullName);
            var repeated = await RunAsync(builds.First, directory.FullName);
            var changed = await RunAsync(builds.Second, directory.FullName);
            await Assert.That(first.ValueOrDefault).IsEqualTo("original");
            await Assert.That(repeated.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
            await Assert.That(changed.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(changed.ValueOrDefault).IsEqualTo("changed");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    public class LoadContextCacheModule : Module<object>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithCacheKeyPart("load-context-cache");

        protected internal override Task<object> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(Activator.CreateInstance(GetType().Assembly.GetType("RuntimeValue")!)!);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Cached_Runtime_Value_Uses_Module_Context(bool identicalBuilds)
    {
        using var builds = new ModuleLoadContextBuilds(typeof(LoadContextCacheModule), identicalBuilds);
        var directory = Directory.CreateTempSubdirectory("ModularPipelines-load-context-cache-");
        try
        {
            foreach (var assembly in new[] { builds.First, builds.Second })
            {
                var moduleType = assembly.GetType("ContextModule")!;
                await RunAsync(moduleType, directory.FullName);
                var repeated = await RunAsync(moduleType, directory.FullName);
                await Assert.That(repeated.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(repeated.ValueOrDefault!.GetType()).IsEqualTo(assembly.GetType("RuntimeValue"));
            }
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    public class InterfaceMemberCacheModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithCacheKeyPart("interface-members");

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var value = GetType().GetProperty("Value")!.GetValue(this)!;
            return Task.FromResult((string) value.GetType().GetProperty("Label")!.GetValue(value)!);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Changed_Interface_Member_Build_Invalidates_Cache(bool inherited)
    {
        using var builds = new InterfaceMemberBuilds(typeof(InterfaceMemberCacheModule), inherited);
        await Assert.That(builds.First.Module.ModuleVersionId).IsEqualTo(builds.Second.Module.ModuleVersionId);
        var directory = Directory.CreateTempSubdirectory("ModularPipelines-interface-member-cache-");
        try
        {
            var first = await RunAsync(builds.First, directory.FullName);
            var repeated = await RunAsync(builds.First, directory.FullName);
            var changed = await RunAsync(builds.Second, directory.FullName);
            await Assert.That(first.ValueOrDefault).IsEqualTo("original");
            await Assert.That(repeated.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
            await Assert.That(changed.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(changed.ValueOrDefault).IsEqualTo("changed");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    public interface IProcessor<T>;

    [JsonConverter(typeof(UnusedConverterFactory))]
    public interface IMarker;

    [JsonConverter(typeof(UnusedConverterFactory))]
    public class MarkerValue;

    public class ConverterCacheModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithCacheKeyPart("unused-converter");

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("marker");
    }

    public class MarkerCacheModule : ConverterCacheModule, IMarker;

    public class MarkerArgumentCacheModule : ConverterCacheModule, IProcessor<MarkerValue>;

    public sealed class UnusedConverterFactory : JsonConverterFactory
    {
        public UnusedConverterFactory(int value) => _ = value;

        public override bool CanConvert(Type typeToConvert) => throw new InvalidOperationException("This interface is not serialized.");

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            throw new InvalidOperationException("This interface is not serialized.");
    }

    [Test]
    [Arguments(typeof(MarkerCacheModule))]
    [Arguments(typeof(MarkerArgumentCacheModule))]
    public async Task Cache_Does_Not_Construct_Unused_Interface_Converters(Type moduleType)
    {
        var directory = Directory.CreateTempSubdirectory("ModularPipelines-unused-converter-cache-");
        try
        {
            var first = await RunAsync(moduleType, directory.FullName);
            var repeated = await RunAsync(moduleType, directory.FullName);
            await Assert.That(first.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(repeated.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
            await Assert.That(repeated.ValueOrDefault).IsEqualTo("marker");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

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

    private static async Task<IModuleResult> RunAsync(Type moduleType, string directory, object? value = null)
    {
        var builder = TestPipelineBuilder.Create()
            .AddModules(moduleType)
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = directory;
                options.CacheDirectory = Path.Combine(directory, "cache");
            });
        if (value is not null)
        {
            builder.Services.AddSingleton(value);
        }

        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync();
        return pipeline.Services.GetRequiredService<IModuleResultRegistry>().GetResult(moduleType)!;
    }
}
