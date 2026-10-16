using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

public class RuntimeDependencyCycleTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Undeclared_Await_Closing_Declared_Cycle_Fails_With_Guidance(bool optionalLookup)
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with { DefaultModuleTimeout = TimeSpan.FromSeconds(10) })
            .AddModule(new CycleReader(optionalLookup, awaitDependency: true))
            .AddModule<DeclaredDependent>()
            .RunAsync()
            .WaitAsync(TestHostSettings.DefaultTestTimeout));

        var cycle = exception!.InnerException as CircularDependencyException;
        await Assert.That(cycle).IsNotNull();
        await Assert.That(cycle!.Message).Contains(nameof(CycleReader));
        await Assert.That(cycle.Message).Contains(nameof(DeclaredDependent));
        await Assert.That(cycle.Message).Contains("DependsOn");
        await Assert.That(cycle.CycleTypes).IsEquivalentTo(new[]
        {
            typeof(CycleReader), typeof(DeclaredDependent), typeof(CycleReader),
        });
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Lookup_Without_Awaiting_Does_Not_Create_A_Cycle(bool optionalLookup)
    {
        await Assert.That(async () => await TestPipelineBuilder.Create()
            .AddModule(new CycleReader(optionalLookup, awaitDependency: false))
            .AddModule<DeclaredDependent>()
            .RunAsync()
            .WaitAsync(TestHostSettings.DefaultTestTimeout)).ThrowsNothing();
    }

    [Test]
    public async Task Cycle_With_Only_Runtime_Waits_Fails()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with { DefaultModuleTimeout = TimeSpan.FromSeconds(10) })
            .AddModule<RuntimeFirst>()
            .AddModule<RuntimeSecond>()
            .RunAsync()
            .WaitAsync(TestHostSettings.DefaultTestTimeout));

        await Assert.That(exception!.ToString()).Contains(nameof(CircularDependencyException));
        await Assert.That(exception.ToString()).Contains(nameof(RuntimeFirst));
        await Assert.That(exception.ToString()).Contains(nameof(RuntimeSecond));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Acyclic_Undeclared_Await_Succeeds(bool optionalLookup)
    {
        await Assert.That(async () => await TestPipelineBuilder.Create()
            .AddModule(new AcyclicReader(optionalLookup))
            .AddModule<Leaf>()
            .RunAsync()
            .WaitAsync(TestHostSettings.DefaultTestTimeout)).ThrowsNothing();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Self_Reference_Still_Fails(bool optionalLookup)
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineBuilder.Create()
            .AddModule(new SelfReader(optionalLookup))
            .RunAsync());

        await Assert.That(exception!.InnerException).IsTypeOf<ModuleSelfDependencyException>();
    }

    [Test]
    public async Task Required_Unregistered_Module_Still_Fails()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineBuilder.Create()
            .AddModule(new AcyclicReader(optionalLookup: false))
            .RunAsync());

        await Assert.That(exception!.InnerException).IsTypeOf<ModuleNotRegisteredException>();
    }

    [Test]
    public async Task Optional_Unregistered_Module_Still_Returns_Null()
    {
        var reader = new AcyclicReader(optionalLookup: true);
        await TestPipelineBuilder.Create().AddModule(reader).RunAsync();
        await Assert.That(reader.DependencyWasNull).IsTrue();
    }

    private sealed class CycleReader(bool optionalLookup, bool awaitDependency) : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var dependency = optionalLookup
                ? context.GetModuleIfRegistered<DeclaredDependent>()!
                : context.GetModule<DeclaredDependent>();
            if (awaitDependency)
            {
                await dependency;
            }

            return true;
        }
    }

    [DependsOn<CycleReader>]
    private sealed class DeclaredDependent : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await context.GetModule<CycleReader>();
            return true;
        }
    }

    private sealed class RuntimeFirst : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await context.GetModule<RuntimeSecond>();
            return true;
        }
    }

    private sealed class RuntimeSecond : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await context.GetModuleIfRegistered<RuntimeFirst>()!;
            return true;
        }
    }

    private sealed class AcyclicReader(bool optionalLookup) : Module<bool>
    {
        public bool DependencyWasNull { get; private set; }

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var dependency = optionalLookup ? context.GetModuleIfRegistered<Leaf>() : context.GetModule<Leaf>();
            DependencyWasNull = dependency is null;
            if (dependency is not null)
            {
                await dependency;
            }

            return true;
        }
    }

    private sealed class Leaf : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }

    private sealed class SelfReader(bool optionalLookup) : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var self = optionalLookup ? context.GetModuleIfRegistered<SelfReader>() : context.GetModule<SelfReader>();
            await self!;
            return true;
        }
    }
}
