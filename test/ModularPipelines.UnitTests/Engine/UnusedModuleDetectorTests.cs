using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class UnusedModuleDetectorTests
{
    private readonly Mock<IPipelineServiceContainerWrapper> _serviceContainerWrapper = new();
    private readonly Mock<IAssemblyLoadedTypesProvider> _assemblyLoadedTypesProvider = new();

    [Test]
    public async Task Loaded_But_Unused_Modules_Are_Debug_Only()
    {
        _assemblyLoadedTypesProvider.Setup(x => x.GetLoadedTypesAssignableTo(typeof(IModule)))
            .Returns([
                typeof(Module1),
                typeof(Module2),
                typeof(Module3),
                typeof(Module4),
                typeof(Module5)
            ]);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddModule<Module1>()
            .AddModule<Module3>()
            .AddModule<Module4>();

        _serviceContainerWrapper.Setup(x => x.ServiceCollection)
            .Returns(serviceCollection);
        var logger = new RecordingLogger<UnusedModuleDetector>();
        var detector = new UnusedModuleDetector(
            _assemblyLoadedTypesProvider.Object,
            _serviceContainerWrapper.Object,
            logger);

        detector.Log();

        var entry = logger.Entries.Single();
        await Assert.That(entry.Level).IsEqualTo(LogLevel.Debug);
        await Assert.That(entry.Message).IsEqualTo("Loaded module types not registered (2): Module2, Module5");
    }

    [Test]
    public async Task Missing_Required_Dependencies_Are_Warnings()
    {
        var logger = new RecordingLogger<UnusedModuleDetector>();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddModule<ModuleWithMissingDependency>();
        _serviceContainerWrapper.Setup(x => x.ServiceCollection).Returns(serviceCollection);
        _assemblyLoadedTypesProvider.Setup(x => x.GetLoadedTypesAssignableTo(typeof(IModule)))
            .Returns([typeof(ModuleWithMissingDependency), typeof(Module2)]);
        var detector = new UnusedModuleDetector(
            _assemblyLoadedTypesProvider.Object,
            _serviceContainerWrapper.Object,
            logger);

        detector.Log();

        var warning = logger.Entries.Single(entry => entry.Level == LogLevel.Warning);
        await Assert.That(warning.Message).Contains("Missing Required Module Dependencies");
        await Assert.That(warning.Message).Contains(nameof(Module2));
        await Assert.That(logger.Entries.Where(entry => entry.Level == LogLevel.Debug))
            .IsEmpty();
    }

    [Test]
    public async Task Log_WhenDiagnosticsDisabled_DoesNotScanAssembliesOrServices()
    {
        var logger = new Mock<ILogger<UnusedModuleDetector>>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);
        logger.Setup(x => x.IsEnabled(LogLevel.Warning)).Returns(false);
        var detector = new UnusedModuleDetector(
            _assemblyLoadedTypesProvider.Object,
            _serviceContainerWrapper.Object,
            logger.Object);

        detector.Log();

        _assemblyLoadedTypesProvider.Verify(
            x => x.GetLoadedTypesAssignableTo(It.IsAny<Type>()),
            Times.Never);
        _serviceContainerWrapper.VerifyGet(x => x.ServiceCollection, Times.Never);
        await Assert.That(logger.Invocations.Where(invocation => invocation.Method.Name == nameof(ILogger.Log)))
            .IsEmpty();
    }

    private class Module1 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class Module2 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class Module3 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class Module4 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class Module5 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.Attributes.DependsOn<Module2>]
    private class ModuleWithMissingDependency : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<(LogLevel Level, string Message)> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Add((logLevel, formatter(state, exception)));
        }
    }
}
