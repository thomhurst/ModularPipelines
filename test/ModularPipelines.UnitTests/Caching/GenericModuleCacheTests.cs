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
    public class VersionOverrideMarkerModule : ConverterCacheModule
    {
        protected override void Configure(ModuleConfigurationBuilder module)
        {
            base.Configure(module);
            module.WithCacheAssemblyVersionKey("stable-interface-module");
        }
    }

    [Test]
    [Arguments(false, false, false)]
    [Arguments(true, false, false)]
    [Arguments(false, true, false)]
    [Arguments(true, true, false)]
    [Arguments(true, false, true)]
    public async Task Same_Assembly_Interface_Respects_Version_Override(bool versionOverride, bool includeMember, bool genericArgument)
    {
        var name = $"VersionedInterface_{Guid.NewGuid():N}";
        var parent = versionOverride ? typeof(VersionOverrideMarkerModule) : typeof(ConverterCacheModule);
        var firstType = CreateVersionedInterfaceModule(name, parent, includeMember, genericArgument);
        var secondType = CreateVersionedInterfaceModule(name, parent, includeMember, genericArgument);
        await Assert.That(firstType.Module.ModuleVersionId).IsNotEqualTo(secondType.Module.ModuleVersionId);
        await Assert.That(ModuleId.FromType(firstType)).IsEqualTo(ModuleId.FromType(secondType));
        var directory = Directory.CreateTempSubdirectory("ModularPipelines-versioned-interface-");
        try
        {
            await RunAsync(firstType, directory.FullName);
            var repeated = await RunAsync(firstType, directory.FullName);
            var rebuilt = await RunAsync(secondType, directory.FullName);
            await Assert.That(repeated.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
            await Assert.That(rebuilt.Status).IsEqualTo(versionOverride && !genericArgument ? ModuleStatus.RestoredFromCache : ModuleStatus.Succeeded);
            await Assert.That(rebuilt.ValueOrDefault).IsEqualTo("marker");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    private static Type CreateVersionedInterfaceModule(string name, Type parent, bool includeMember, bool genericArgument)
    {
        var module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.RunAndCollect).DefineDynamicModule(name);
        var value = module.DefineType("LocalValue", TypeAttributes.Public).CreateType()!;
        var contract = module.DefineType("ILocalContract", TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
        if (includeMember)
        {
            contract.DefineMethod("GetValue", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Abstract, value, Type.EmptyTypes);
        }

        var declaredContract = contract.CreateType()!;
        var implementation = module.DefineType("VersionedModule", TypeAttributes.Public, parent);
        implementation.AddInterfaceImplementation(genericArgument ? typeof(IProcessor<>).MakeGenericType(value) : declaredContract);
        if (includeMember)
        {
            var getter = implementation.DefineMethod("GetValue", MethodAttributes.Public | MethodAttributes.Virtual, value, Type.EmptyTypes);
            var il = getter.GetILGenerator();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);
        }

        return implementation.CreateType()!;
    }

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

    public class VersionOverrideInterfaceMemberCacheModule : InterfaceMemberCacheModule
    {
        protected override void Configure(ModuleConfigurationBuilder module)
        {
            base.Configure(module);
            module.WithCacheAssemblyVersionKey("stable-interface-members");
        }
    }

    [Test]
    [Arguments(false, false, false, false)]
    [Arguments(true, false, false, false)]
    [Arguments(false, true, false, false)]
    [Arguments(true, true, false, false)]
    [Arguments(false, false, true, false)]
    [Arguments(true, false, true, false)]
    [Arguments(false, true, true, false)]
    [Arguments(true, true, true, false)]
    [Arguments(false, false, false, true)]
    [Arguments(true, false, false, true)]
    [Arguments(false, true, false, true)]
    [Arguments(true, true, false, true)]
    public async Task Changed_Module_Contract_Build_Invalidates_Cache(bool inherited, bool versionOverride, bool typeConstraint, bool moduleConstraint)
    {
        using var builds = new InterfaceMemberBuilds(versionOverride ? typeof(VersionOverrideInterfaceMemberCacheModule) : typeof(InterfaceMemberCacheModule), inherited, typeConstraint, moduleConstraint);
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

    public class ConstraintValue : IMarker;

    public class ConstraintCacheModule<T> : ConverterCacheModule where T : IMarker;

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
    [Arguments(typeof(ConstraintCacheModule<ConstraintValue>))]
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
