using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
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
    public async Task GetModule_BuildsModuleLookupOnlyOnce()
    {
        var module = new FirstLookupModule();
        var resolutionCount = 0;
        var serviceProvider = CreateServiceProvider([module], () => resolutionCount++);
        using var engineCancellationToken = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(serviceProvider, engineCancellationToken);

        var firstResult = context.GetModule<FirstLookupModule>();
        var secondResult = context.GetModule<FirstLookupModule>();

        await Assert.That(firstResult).IsSameReferenceAs(module);
        await Assert.That(secondResult).IsSameReferenceAs(module);
        await Assert.That(resolutionCount).IsEqualTo(1);
    }

    [Test]
    public async Task GetModule_ReturnsOnlyAssignableModule()
    {
        var module = new FirstLookupModule();
        var serviceProvider = CreateServiceProvider([module]);
        using var engineCancellationToken = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(serviceProvider, engineCancellationToken);

        var result = context.GetModule<LookupModule>();

        await Assert.That(result).IsSameReferenceAs(module);
    }

    [Test]
    public async Task GetModuleByType_RequiresExactModuleType()
    {
        var serviceProvider = CreateServiceProvider([new FirstLookupModule()]);
        using var engineCancellationToken = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(serviceProvider, engineCancellationToken);

        var result = context.GetModule(typeof(LookupModule));

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task GetModule_WhenMultipleModulesMatch_ThrowsDescriptivePipelineException()
    {
        var serviceProvider = CreateServiceProvider([new FirstLookupModule(), new SecondLookupModule()]);
        using var engineCancellationToken = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(serviceProvider, engineCancellationToken);

        var exception = Assert.Throws<AmbiguousModuleException>(() => context.GetModule<LookupModule>());

        await Assert.That(exception.Message).Contains(nameof(LookupModule));
        await Assert.That(exception.Message).Contains(nameof(FirstLookupModule));
        await Assert.That(exception.Message).Contains(nameof(SecondLookupModule));
        await Assert.That(exception.RequestedType).IsEqualTo(typeof(LookupModule));
        await Assert.That(exception.MatchingModuleTypes)
            .IsEquivalentTo([typeof(FirstLookupModule), typeof(SecondLookupModule)]);
    }

    private static Mock<IServiceProvider> CreateServiceProvider(
        IReadOnlyList<IModule> modules,
        Action? onResolve = null)
    {
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider
            .Setup(x => x.GetService(typeof(IEnumerable<IModule>)))
            .Returns(() =>
            {
                onResolve?.Invoke();
                return modules;
            });

        return serviceProvider;
    }

    private static PipelineContext CreateContext(
        Mock<IServiceProvider> serviceProvider,
        EngineCancellationToken engineCancellationToken)
    {
        return new PipelineContext(
            serviceProvider.Object,
            Mock.Of<IDependencyCollisionDetector>(),
            Mock.Of<IModuleResultRepository>(),
            Mock.Of<IInternalModuleLoggerProvider>(),
            engineCancellationToken,
            Mock.Of<IShellContext>(),
            Mock.Of<IFilesContext>(),
            Mock.Of<IDataContext>(),
            Mock.Of<IEnvironmentDomainContext>(),
            Mock.Of<IInstallersContext>(),
            Mock.Of<INetworkContext>(),
            Mock.Of<ISecurityContext>(),
            Mock.Of<IServicesContext>(),
            Mock.Of<ISummaryLogger>());
    }

    private abstract class LookupModule : Module<string>;

    private sealed class FirstLookupModule : LookupModule
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string?>(nameof(FirstLookupModule));
    }

    private sealed class SecondLookupModule : LookupModule
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string?>(nameof(SecondLookupModule));
    }
}
