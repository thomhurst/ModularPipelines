using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using Moq;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.Context;

public class PipelineContextModuleLookupTests
{
    [Test]
    public async Task ModuleLookup_IsSharedAcrossScopes()
    {
        var module = new FirstLookupModule();
        var services = new ServiceCollection();
        DependencyInjectionSetup.Initialize(services);
        services.AddSingleton<IModule>(module);
        await using var serviceProvider = services.BuildServiceProvider();
        using var firstScope = serviceProvider.CreateScope();
        using var secondScope = serviceProvider.CreateScope();

        var firstLookup = firstScope.ServiceProvider.GetRequiredService<ModuleLookup>();
        var secondLookup = secondScope.ServiceProvider.GetRequiredService<ModuleLookup>();

        await Assert.That(secondLookup).IsSameReferenceAs(firstLookup);
        await Assert.That(firstLookup.GetAssignable(typeof(FirstLookupModule)))
            .IsSameReferenceAs(module);
    }

    [Test]
    public async Task GetModule_ReturnsOnlyAssignableModule()
    {
        var module = new FirstLookupModule();
        var moduleLookup = CreateModuleLookup([module]);
        using var engineCancellationToken = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(moduleLookup, engineCancellationToken);

        var result = context.GetModule<LookupModule>();

        await Assert.That(result).IsSameReferenceAs(module);
    }

    [Test]
    public async Task GetModuleByType_RequiresExactModuleType()
    {
        var moduleLookup = CreateModuleLookup([new FirstLookupModule()]);
        using var engineCancellationToken = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(moduleLookup, engineCancellationToken);

        var result = context.GetModule(typeof(LookupModule));

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task GetModule_WhenMultipleModulesMatch_ThrowsDescriptivePipelineException()
    {
        var moduleLookup = CreateModuleLookup([new FirstLookupModule(), new SecondLookupModule()]);
        using var engineCancellationToken = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(moduleLookup, engineCancellationToken);

        var exception = Assert.Throws<AmbiguousModuleException>(() => context.GetModule<LookupModule>());

        await Assert.That(exception.Message).Contains(nameof(LookupModule));
        await Assert.That(exception.Message).Contains(nameof(FirstLookupModule));
        await Assert.That(exception.Message).Contains(nameof(SecondLookupModule));
        await Assert.That(exception.RequestedType).IsEqualTo(typeof(LookupModule));
        await Assert.That(exception.MatchingModuleTypes)
            .IsEquivalentTo([typeof(FirstLookupModule), typeof(SecondLookupModule)]);
    }

    [Test]
    public async Task GetModule_WhenConcreteBaseAndDerivedModulesMatch_ThrowsDescriptivePipelineException()
    {
        var moduleLookup = CreateModuleLookup(
            [new ConcreteBaseLookupModule(), new DerivedConcreteLookupModule()]);
        using var engineCancellationToken = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(moduleLookup, engineCancellationToken);

        var exception = Assert.Throws<AmbiguousModuleException>(
            () => context.GetModule<ConcreteBaseLookupModule>());

        await Assert.That(exception.RequestedType).IsEqualTo(typeof(ConcreteBaseLookupModule));
        await Assert.That(exception.MatchingModuleTypes)
            .IsEquivalentTo([typeof(ConcreteBaseLookupModule), typeof(DerivedConcreteLookupModule)]);
    }

    private static ModuleLookup CreateModuleLookup(IReadOnlyList<IModule> modules) =>
        new(modules);

    private static PipelineContext CreateContext(
        ModuleLookup moduleLookup,
        EngineCancellationToken engineCancellationToken)
    {
        return new PipelineContext(
            moduleLookup,
            Mock.Of<IDependencyCollisionDetector>(),
            Mock.Of<IModuleResultRepository>(),
            Mock.Of<IInternalModuleLoggerProvider>(),
            engineCancellationToken,
            Mock.Of<IShellContext>(),
            Mock.Of<IFilesContext>(),
            Mock.Of<IDataContext>(),
            Mock.Of<IEnvironmentContext>(),
            Mock.Of<IInstallersContext>(),
            Mock.Of<INetworkContext>(),
            Mock.Of<ISecurityContext>(),
            Mock.Of<IServicesContext>(),
            Mock.Of<ISummaryLogger>());
    }

    private abstract class LookupModule : Module<string>;

    private sealed class FirstLookupModule : LookupModule
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string>(nameof(FirstLookupModule));
    }

    private sealed class SecondLookupModule : LookupModule
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string>(nameof(SecondLookupModule));
    }

    private class ConcreteBaseLookupModule : LookupModule
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string>(nameof(ConcreteBaseLookupModule));
    }

    private sealed class DerivedConcreteLookupModule : ConcreteBaseLookupModule;
}
