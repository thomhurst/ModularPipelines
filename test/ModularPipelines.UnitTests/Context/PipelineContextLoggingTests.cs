using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;
using Moq;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.Context;

public class PipelineContextLoggingTests
{
    [Test]
    public async Task Reused_Module_Context_Uses_Module_Logger_After_Early_Pipeline_Logging()
    {
        using var cancellation = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        await using var services = new ServiceCollection()
            .AddScoped<IPipelineContext>(_ => CreateContext(new ModuleLoggerAccessor(
                Mock.Of<IServiceProvider>(),
                Mock.Of<IStackTraceModuleDetector>(),
                NullLoggerFactory.Instance,
                Mock.Of<ISecretObfuscator>(),
                Mock.Of<IFormattedLogValuesObfuscator>()), cancellation))
            .BuildServiceProvider();
        await using var provider = new ModuleContextProvider(services);
        var firstOwner = new object();
        var secondOwner = new object();
        var firstContext = provider.GetModuleContext(firstOwner);
        var secondContext = provider.GetModuleContext(secondOwner);

        // Ready hooks and conditions run before a module output scope exists.
        await Assert.That(firstContext.Logger).IsTypeOf<PipelineLevelLogger>();
        await Assert.That(secondContext.Logger).IsTypeOf<PipelineLevelLogger>();

        var firstLogger = Mock.Of<IModuleLogger>();
        using (new ModuleOutputContextScope(typeof(FirstModule), firstLogger))
        {
            var reusedContext = provider.GetModuleContext(firstOwner);
            await Assert.That(reusedContext).IsSameReferenceAs(firstContext);
            await Assert.That(reusedContext.Logger).IsSameReferenceAs(firstLogger);
        }

        var secondLogger = Mock.Of<IModuleLogger>();
        using (new ModuleOutputContextScope(typeof(SecondModule), secondLogger))
        {
            await Assert.That(provider.GetModuleContext(secondOwner).Logger).IsSameReferenceAs(secondLogger);
        }

        await Assert.That(provider.GetScopes().Count()).IsEqualTo(2);
    }

    [Test]
    public async Task Explicitly_Initialized_Logger_Remains_Bound_To_Module()
    {
        var logger = Mock.Of<IModuleLogger>();
        var accessor = new Mock<IInternalModuleLoggerAccessor>(MockBehavior.Strict);
        accessor.Setup(value => value.GetLogger(typeof(FirstModule))).Returns(logger);
        using var cancellation = new EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var context = CreateContext(accessor.Object, cancellation);

        context.InitializeLogger(typeof(FirstModule));

        await Assert.That(context.Logger).IsSameReferenceAs(logger);
        await Assert.That(context.Logger).IsSameReferenceAs(logger);
        accessor.Verify(value => value.GetLogger(typeof(FirstModule)), Times.Once);
    }

    private static PipelineContext CreateContext(
        IInternalModuleLoggerAccessor accessor,
        EngineCancellationToken cancellation) => new(
        new ModuleLookup([]),
        Mock.Of<IDependencyCollisionDetector>(),
        Mock.Of<IModuleResultRepository>(),
        accessor,
        Mock.Of<IConsoleWriter>(),
        cancellation,
        Mock.Of<IShellContext>(),
        Mock.Of<IFilesContext>(),
        Mock.Of<IDataContext>(),
        Mock.Of<IEnvironmentContext>(),
        Mock.Of<IInstallersContext>(),
        Mock.Of<INetworkContext>(),
        Mock.Of<ISecurityContext>(),
        Mock.Of<IServicesContext>(),
        Mock.Of<IArtifactContext>(),
        Mock.Of<ISummaryLogger>());

    private sealed class FirstModule;

    private sealed class SecondModule;
}
