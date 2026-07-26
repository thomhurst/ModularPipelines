using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Modules;

/// <summary>
/// Covers the run-time validation overload that separates the modules being validated from the
/// universe of available modules, so the runnable set can be revalidated without misreporting a
/// dependency on a registered-but-skipped module as missing.
/// </summary>
public class ModuleDependencyValidatorTests
{
    [Test]
    public async Task Runnable_Module_Depending_On_Registered_But_Skipped_Module_Does_Not_Throw()
    {
        var consumer = new ConsumerModule();
        var producer = new ProducerModule();

        // The consumer is the only runnable module; the producer is registered but skipped
        // (e.g. an OS-only module on a foreign OS), so it is available but not in the runnable set.
        await Assert.That(() => ModuleDependencyValidator.Validate(
                modulesToValidate: new IModule[] { consumer },
                availableModules: new IModule[] { consumer, producer },
                dynamicRegistry: null,
                metadataRegistry: null))
            .ThrowsNothing();
    }

    [Test]
    public async Task Runnable_Module_Depending_On_Unavailable_Module_Throws()
    {
        var consumer = new ConsumerModule();

        // The producer is neither runnable nor registered, so the required dependency is genuinely
        // unsatisfiable and must be reported.
        await Assert.That(() => ModuleDependencyValidator.Validate(
                modulesToValidate: new IModule[] { consumer },
                availableModules: new IModule[] { consumer },
                dynamicRegistry: null,
                metadataRegistry: null))
            .Throws<ModuleNotRegisteredException>();
    }

    private sealed class ProducerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("produced");
    }

    [DependsOn<ProducerModule>]
    private sealed class ConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("consumed");
    }
}
