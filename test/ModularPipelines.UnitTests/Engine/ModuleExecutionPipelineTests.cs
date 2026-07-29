using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;
using OptionsFactory = Microsoft.Extensions.Options.Options;
using PipelineEngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleExecutionPipelineTests
{
    private class SuccessfulModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(42);
        }
    }

    [Test]
    public async Task ExecuteAsync_DisposesOriginalAndLinkedCancellationTokenSources()
    {
        var module = new SuccessfulModule();
        var executionContext = new ModuleExecutionContext<int>(module, module.GetType());
        var originalCancellationTokenSource = executionContext.ModuleCancellationTokenSource;
        var logger = new Mock<IModuleLogger>();
        var services = new Mock<IServicesContext>();
        services.SetupGet(x => x.Options).Returns(new PipelineOptions());
        var moduleContext = new Mock<IModuleContext>();
        moduleContext.SetupGet(x => x.Logger).Returns(logger.Object);
        moduleContext.SetupGet(x => x.Services).Returns(services.Object);

        var resultRepository = new Mock<IModuleResultRepository>();
        resultRepository.SetupGet(x => x.IsEnabled).Returns(false);
        var directHookInvoker = new Mock<IDirectHookInvoker>();
        directHookInvoker
            .Setup(x => x.InvokeBeforeExecuteAsync(
                module,
                moduleContext.Object,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        directHookInvoker
            .Setup(x => x.InvokeAfterExecuteAsync(
                module,
                moduleContext.Object,
                It.IsAny<ModuleResult<int>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleResult<int>?) null);
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var pipeline = new ModuleExecutionPipeline(
            resultRepository.Object,
            engineCancellationToken,
            directHookInvoker.Object,
            OptionsFactory.Create(new PipelineOptions()));

        await pipeline.ExecuteAsync(
            module,
            executionContext,
            moduleContext.Object,
            CancellationToken.None);

        var linkedCancellationTokenSource = executionContext.ModuleCancellationTokenSource;
        Assert.Throws<ObjectDisposedException>(() => _ = originalCancellationTokenSource.Token);
        Assert.Throws<ObjectDisposedException>(() => _ = linkedCancellationTokenSource.Token);
    }
}
