using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Extensions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class GeneratedModuleMetadataTests
{
    [Test]
    public async Task Generated_Dependencies_Are_Used_For_Dependency_Only_Registration()
    {
        var services = new ServiceCollection();
        services.AddModule<GeneratedMetadataDependentModule>();

        var found = GeneratedModuleMetadata.TryGetDependencies(
            typeof(GeneratedMetadataDependentModule),
            out var dependencies);
        ModuleAutoRegistrar.AutoRegisterMissingDependencies(services);
        var registeredTypes = ServiceCollectionExtensions.GetRegisteredModuleTypes(services);

        using (Assert.Multiple())
        {
            await Assert.That(found).IsTrue();
            await Assert.That(dependencies)
                .Contains(dependency => dependency.DependencyType == typeof(GeneratedMetadataDependencyModule)
                                        && !dependency.Optional);
            await Assert.That(registeredTypes).Contains(typeof(GeneratedMetadataDependencyModule));
        }
    }

    [Test]
    public async Task Generated_Registration_Preserves_Duplicate_Module_Behavior()
    {
        var services = new ServiceCollection();

        var first = GeneratedModuleMetadata.TryRegisterModule(
            services,
            typeof(GeneratedMetadataDependencyModule));
        var second = GeneratedModuleMetadata.TryRegisterModule(
            services,
            typeof(GeneratedMetadataDependencyModule));

        using (Assert.Multiple())
        {
            await Assert.That(first).IsTrue();
            await Assert.That(second).IsTrue();
            await Assert.That(services.Count(descriptor => descriptor.ServiceType == typeof(IModule)))
                .IsEqualTo(2);
        }
    }

    [Test]
    public async Task Generated_Runtime_Creates_Typed_Terminated_Result()
    {
        var module = new GeneratedMetadataDependencyModule();
        var registry = new ModuleResultRegistry();
        var registrar = new ModuleResultRegistrar(
            registry,
            NullLogger<ModuleResultRegistrar>.Instance);
        var exception = new InvalidOperationException("Pipeline terminated");

        registrar.RegisterTerminatedResult(module, module.GetType(), exception);

        var result = registry.GetResult(module.GetType());
        var awaitedResult = await ((IInternalModule) module).ResultTask.WaitAsync(TimeSpan.FromSeconds(1));
        await Assert.That(result).IsAssignableTo<ModuleResult<bool>>();
        await Assert.That(result!.ExceptionOrDefault).IsSameReferenceAs(exception);
        await Assert.That(awaitedResult).IsSameReferenceAs(result);
    }

    [Test]
    public async Task Cancelled_Result_Registration_Defers_AlwaysRun_Completion()
    {
        var module = new GeneratedAlwaysRunModule();
        var registry = new ModuleResultRegistry();
        var registrar = new ModuleResultRegistrar(
            registry,
            NullLogger<ModuleResultRegistrar>.Instance);

        registrar.RegisterTerminatedResultsForCancelledModules(
            [module],
            new InvalidOperationException("Pipeline terminated"));

        using (Assert.Multiple())
        {
            await Assert.That(registry.GetResult(module.GetType())).IsNull();
            await Assert.That(module.CompletionSource.Task.IsCompleted).IsFalse();
        }
    }

    [Test]
    public async Task Generated_Runtime_Cancels_Typed_Completion_Source()
    {
        var module = new GeneratedMetadataDependencyModule();

        var found = GeneratedModuleMetadata.TryGetRuntime(module.GetType(), out var runtime);
        runtime.CancelCompletionSource(module);

        using (Assert.Multiple())
        {
            await Assert.That(found).IsTrue();
            await Assert.That(module.CompletionSource.Task.IsCanceled).IsTrue();
        }
    }

    [Test]
    public async Task Generated_Runtime_Resolves_Unbuffered_Output_Logger()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        using var serviceProvider = services.BuildServiceProvider();

        var found = GeneratedModuleMetadata.TryGetRuntime(
            typeof(GeneratedMetadataDependencyModule),
            out var runtime);
        var logger = runtime.GetOutputLogger(serviceProvider);

        using (Assert.Multiple())
        {
            await Assert.That(found).IsTrue();
            await Assert.That(logger)
                .IsAssignableTo<ILogger<GeneratedMetadataDependencyModule>>();
            await Assert.That(logger).IsNotAssignableTo<ModuleLogger>();
        }
    }

    [Test]
    public async Task Dynamic_Assembly_Uses_Reflection_Fallback()
    {
        var (assembly, _, module) = CreateDynamicModule("DynamicModule");

        var knownTypes = AssemblyLoadedTypesProvider
            .GetKnownTypes(assembly, typeof(IModule))
            .ToArray();
        var services = new ServiceCollection();
        services.AddModulesFromAssembly(assembly);

        using (Assert.Multiple())
        {
            await Assert.That(knownTypes).Contains(module);
            await Assert.That(ServiceCollectionExtensions.GetRegisteredModuleTypes(services))
                .Contains(module);
        }
    }

    [Test]
    public async Task Complete_Generated_Metadata_Is_Preferred_Over_Assembly_Scan()
    {
        var (_, moduleBuilder, includedModule) = CreateDynamicModule("IncludedModule");
        _ = CreateDynamicModule(moduleBuilder, "ExcludedModule");
        var assembly = includedModule.Assembly;
        GeneratedModuleMetadata.Register(
            assembly,
            [
                new GeneratedModuleRegistration(
                    includedModule,
                    static _ => { },
                    [],
                    DependenciesComplete: true),
            ],
            isComplete: true);

        var knownTypes = AssemblyLoadedTypesProvider
            .GetKnownTypes(assembly, typeof(IModule))
            .ToArray();
        var generatedKnownTypes = AssemblyLoadedTypesProvider
            .GetGeneratedKnownTypes(assembly, typeof(IModule))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(knownTypes).IsEquivalentTo([includedModule]);
            await Assert.That(generatedKnownTypes).IsEquivalentTo([includedModule]);
        }
    }

    [Test]
    public async Task Incomplete_Generated_Metadata_Is_Skipped_Without_Reflection_Fallback()
    {
        var (_, _, module) = CreateDynamicModule("IncompleteModule");
        var assembly = module.Assembly;
        GeneratedModuleMetadata.Register(
            assembly,
            [
                new GeneratedModuleRegistration(
                    module,
                    static _ => { },
                    [],
                    DependenciesComplete: true),
            ],
            isComplete: false);

        var knownTypes = AssemblyLoadedTypesProvider
            .GetKnownTypes(assembly, typeof(IModule))
            .ToArray();
        var generatedKnownTypes = AssemblyLoadedTypesProvider
            .GetGeneratedKnownTypes(assembly, typeof(IModule))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(knownTypes).Contains(module);
            await Assert.That(generatedKnownTypes).IsEmpty();
        }
    }

    private static (AssemblyBuilder Assembly, ModuleBuilder ModuleBuilder, Type Module)
        CreateDynamicModule(string name)
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName($"DynamicModules_{Guid.NewGuid():N}"),
            AssemblyBuilderAccess.Run);
        var module = assembly.DefineDynamicModule("Main");
        return (assembly, module, CreateDynamicModule(module, name));
    }

    private static Type CreateDynamicModule(ModuleBuilder module, string name)
    {
        var typeBuilder = module.DefineType(
            name,
            TypeAttributes.Public | TypeAttributes.Class,
            typeof(TrueModule));
        var constructor = typeBuilder.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            Type.EmptyTypes);
        var il = constructor.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Call, typeof(TrueModule).GetConstructor(Type.EmptyTypes)!);
        il.Emit(OpCodes.Ret);
        return typeBuilder.CreateType()!;
    }

    private sealed class GeneratedAlwaysRunModule : Module<bool>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithAlwaysRun()
            .Build();

        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}

public sealed class GeneratedMetadataDependencyModule : TrueModule;

[ModularPipelines.Attributes.DependsOn<GeneratedMetadataDependencyModule>]
public sealed class GeneratedMetadataDependentModule : TrueModule;
