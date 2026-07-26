using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Modules;

/// <summary>
/// Covers dependency validation over the dynamic (registration-event) dependency registry, which
/// the executors invoke against the runnable set once registration events have populated it.
/// </summary>
public class ModuleDependencyValidatorTests
{
    [Test]
    public async Task Validate_Dynamic_Dependency_On_Module_Outside_The_Set_Throws()
    {
        var consumer = new ConsumerModule();

        var dynamicRegistry = new ModuleDependencyRegistry();
        dynamicRegistry.AddDynamicDependency(typeof(ConsumerModule), typeof(ProducerModule));

        // The producer is not among the validated modules, so the required dynamic dependency is
        // unsatisfiable and must be reported eagerly (mirrors the DependencyWaiter failure).
        await Assert.That(() => ModuleDependencyValidator.Validate(
                new IModule[] { consumer },
                dynamicRegistry,
                metadataRegistry: null))
            .Throws<ModuleNotRegisteredException>();
    }

    [Test]
    public async Task Validate_Dynamic_Dependency_On_Module_In_The_Set_Does_Not_Throw()
    {
        var consumer = new ConsumerModule();
        var producer = new ProducerModule();

        var dynamicRegistry = new ModuleDependencyRegistry();
        dynamicRegistry.AddDynamicDependency(typeof(ConsumerModule), typeof(ProducerModule));

        await Assert.That(() => ModuleDependencyValidator.Validate(
                new IModule[] { consumer, producer },
                dynamicRegistry,
                metadataRegistry: null))
            .ThrowsNothing();
    }

    private sealed class ProducerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("produced");
    }

    private sealed class ConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("consumed");
    }
}
