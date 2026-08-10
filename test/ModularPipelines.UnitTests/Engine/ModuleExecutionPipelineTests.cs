using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Caching;
using ModularPipelines.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
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
    private sealed class TrackingCacheRepository : IModuleCacheResultRepository
    {
        public int DiscardCount { get; private set; }

        public CancellationToken ReadCancellationToken { get; private set; }

        public CancellationToken WriteCancellationToken { get; private set; }

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext,
            CancellationToken cancellationToken)
        {
            WriteCancellationToken = cancellationToken;
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext,
            CancellationToken cancellationToken)
        {
            ReadCancellationToken = cancellationToken;
            return Task.FromResult<ModuleResult<T>?>(null);
        }

        public void DiscardFingerprint(IModule module)
        {
            DiscardCount++;
        }
    }

    private class SuccessfulModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(42);
        }
    }

    private sealed class TimeoutExceptionModule : Module<int>
    {
        private readonly TaskCompletionSource _executionStarted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _throwTimeout =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task ExecutionStarted => _executionStarted.Task;

        public void ThrowTimeout() => _throwTimeout.TrySetResult();

        protected internal override async Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            _executionStarted.TrySetResult();
            await _throwTimeout.Task.ConfigureAwait(false);
            throw new ModuleTimeoutException(
                GetType(),
                TimeSpan.FromSeconds(1));
        }
    }

    private sealed class ElapsedCancellationModule : Module<int>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMilliseconds(5))
            .Build();

        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromException<int>(new OperationCanceledException());
        }
    }

    private sealed class AlwaysRunTimeoutExceptionModule : Module<int>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithAlwaysRun()
            .Build();

        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromException<int>(new ModuleTimeoutException(
                GetType(),
                TimeSpan.FromSeconds(1)));
        }
    }

    private sealed class AlwaysRunElapsedCancellationModule : Module<int>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithAlwaysRun()
            .WithTimeout(TimeSpan.FromMilliseconds(5))
            .Build();

        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromException<int>(new OperationCanceledException());
        }
    }

    [Test]
    public async Task ExecuteAsync_ClassifiesLateTimeoutAsPipelineTerminated()
    {
        var module = new TimeoutExceptionModule();
        var result = await ExecuteAfterPipelineCancellation(
            module,
            executionStarted: module.ExecutionStarted,
            releaseExecution: module.ThrowTimeout);

        await Assert.That(result.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
    }

    [Test]
    public async Task ExecuteAsync_ClassifiesElapsedCancellationAsPipelineTerminated()
    {
        var module = new ElapsedCancellationModule();
        var executionContext = new ModuleExecutionContext<int>(module, module.GetType());
        executionContext.Stopwatch.Start();
        await Task.Delay(TimeSpan.FromMilliseconds(25));
        executionContext.Stopwatch.Stop();

        var result = await ExecuteAfterPipelineCancellation(module, executionContext);

        await Assert.That(result.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
    }

    [Test]
    public async Task ExecuteAsync_ClassifiesAlwaysRunTimeoutIndependentlyOfPipelineCancellation()
    {
        var module = new AlwaysRunTimeoutExceptionModule();
        var executionContext = new ModuleExecutionContext<int>(module, module.GetType());

        await Assert.That(async () => await ExecuteAfterPipelineCancellation(module, executionContext))
            .Throws<ModuleFailedException>();

        await Assert.That(executionContext.Status).IsEqualTo(Status.TimedOut);
    }

    [Test]
    public async Task ExecuteAsync_ClassifiesAlwaysRunElapsedCancellationAsTimeout()
    {
        var module = new AlwaysRunElapsedCancellationModule();
        var executionContext = new ModuleExecutionContext<int>(module, module.GetType());
        executionContext.Stopwatch.Start();
        await Task.Delay(TimeSpan.FromMilliseconds(25));
        executionContext.Stopwatch.Stop();

        await Assert.That(async () => await ExecuteAfterPipelineCancellation(module, executionContext))
            .Throws<ModuleFailedException>();

        await Assert.That(executionContext.Status).IsEqualTo(Status.TimedOut);
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
        var moduleConditionHandler = new Mock<IModuleConditionHandler>();
        moduleConditionHandler
            .Setup(x => x.ShouldIgnore(module, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null));
        var pipeline = new ModuleExecutionPipeline(
            resultRepository.Object,
            engineCancellationToken,
            directHookInvoker.Object,
            moduleConditionHandler.Object,
            OptionsFactory.Create(new PipelineOptions()));

        await pipeline.ExecuteAsync(
            module,
            executionContext,
            moduleContext.Object,
            CancellationToken.None);

        var linkedCancellationTokenSource = executionContext.ModuleCancellationTokenSource;
        Assert.Throws<ObjectDisposedException>(() => _ = originalCancellationTokenSource.Token);
        Assert.Throws<ObjectDisposedException>(() => _ = linkedCancellationTokenSource.Token);
        logger.Verify(x => x.Log(
            LogLevel.Trace,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) => state.ToString()!.StartsWith("No module timeout configured.")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        logger.Verify(x => x.Log(
            LogLevel.Debug,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) => state.ToString()!.StartsWith("No module timeout configured.")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Never);
    }

    private sealed class CachedSuccessfulModule : SuccessfulModule
    {
        protected override ModularPipelines.Configuration.ModuleConfiguration Configure() =>
            ModularPipelines.Configuration.ModuleConfiguration.Create()
                .WithCacheKeyPart("v1")
                .Build();
    }

    [Test]
    public async Task ExecuteAsync_LogsExpectedSkipStatusAsInformation()
    {
        var module = new SuccessfulModule();
        var executionContext = new ModuleExecutionContext<int>(module, module.GetType());
        var logger = new Mock<IModuleLogger>();
        var services = new Mock<IServicesContext>();
        services.SetupGet(x => x.Options).Returns(new PipelineOptions());
        var moduleContext = new Mock<IModuleContext>();
        moduleContext.SetupGet(x => x.Logger).Returns(logger.Object);
        moduleContext.SetupGet(x => x.Services).Returns(services.Object);

        var resultRepository = new Mock<IModuleResultRepository>();
        resultRepository.SetupGet(x => x.IsEnabled).Returns(false);
        var skipDecision = SkipDecision.Skip("Configured skip");
        var directHookInvoker = new Mock<IDirectHookInvoker>();
        directHookInvoker
            .Setup(x => x.InvokeSkippedAsync(
                module,
                moduleContext.Object,
                skipDecision,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var moduleConditionHandler = new Mock<IModuleConditionHandler>();
        moduleConditionHandler
            .Setup(x => x.ShouldIgnore(module, It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, skipDecision));
        var pipeline = new ModuleExecutionPipeline(
            resultRepository.Object,
            engineCancellationToken,
            directHookInvoker.Object,
            moduleConditionHandler.Object,
            OptionsFactory.Create(new PipelineOptions()));

        await pipeline.ExecuteAsync(
            module,
            executionContext,
            moduleContext.Object,
            CancellationToken.None);

        var expectedMessage = StatusDisplayProvider.FormatStatusMessage(
            nameof(SuccessfulModule),
            Status.Skipped);
        logger.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) => state.ToString() == expectedMessage),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_DiscardsPendingCacheFingerprint()
    {
        var module = new SuccessfulModule();
        var executionContext = new ModuleExecutionContext<int>(module, module.GetType());
        var logger = new Mock<IModuleLogger>();
        var services = new Mock<IServicesContext>();
        services.SetupGet(x => x.Options).Returns(new PipelineOptions());
        var moduleContext = new Mock<IModuleContext>();
        moduleContext.SetupGet(x => x.Logger).Returns(logger.Object);
        moduleContext.SetupGet(x => x.Services).Returns(services.Object);

        var repository = new TrackingCacheRepository();
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
        var moduleConditionHandler = new Mock<IModuleConditionHandler>();
        moduleConditionHandler
            .Setup(x => x.ShouldIgnore(module, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null));
        var pipeline = new ModuleExecutionPipeline(
            resultRepository.Object,
            engineCancellationToken,
            directHookInvoker.Object,
            moduleConditionHandler.Object,
            OptionsFactory.Create(new PipelineOptions()),
            repository);

        await pipeline.ExecuteAsync(
            module,
            executionContext,
            moduleContext.Object,
            CancellationToken.None);

        await Assert.That(repository.DiscardCount).IsEqualTo(1);
    }

    [Test]
    public async Task ExecuteAsync_PassesModuleCancellationToCacheOperations()
    {
        var module = new CachedSuccessfulModule();
        var executionContext = new ModuleExecutionContext<int>(module, module.GetType());
        var logger = new Mock<IModuleLogger>();
        var services = new Mock<IServicesContext>();
        services.SetupGet(x => x.Options).Returns(new PipelineOptions());
        var moduleContext = new Mock<IModuleContext>();
        moduleContext.SetupGet(x => x.Logger).Returns(logger.Object);
        moduleContext.SetupGet(x => x.Services).Returns(services.Object);

        var resultRepository = new Mock<IModuleResultRepository>();
        resultRepository.SetupGet(x => x.IsEnabled).Returns(false);
        var cacheRepository = new TrackingCacheRepository();
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
        var moduleConditionHandler = new Mock<IModuleConditionHandler>();
        moduleConditionHandler
            .Setup(x => x.ShouldIgnore(module, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null));
        var pipeline = new ModuleExecutionPipeline(
            resultRepository.Object,
            engineCancellationToken,
            directHookInvoker.Object,
            moduleConditionHandler.Object,
            OptionsFactory.Create(new PipelineOptions()),
            cacheRepository);

        await pipeline.ExecuteAsync(
            module,
            executionContext,
            moduleContext.Object,
            CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(cacheRepository.ReadCancellationToken.CanBeCanceled).IsTrue();
            await Assert.That(cacheRepository.WriteCancellationToken)
                .IsEqualTo(cacheRepository.ReadCancellationToken);
        }
    }

    private static async Task<ModuleResult<int>> ExecuteAfterPipelineCancellation(
        Module<int> module,
        ModuleExecutionContext<int>? executionContext = null,
        Task? executionStarted = null,
        Action? releaseExecution = null)
    {
        executionContext ??= new ModuleExecutionContext<int>(module, module.GetType());

        var logger = new Mock<IInternalModuleLogger>();
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
            .Setup(x => x.InvokeFailedAsync(
                module,
                moduleContext.Object,
                It.IsAny<Exception>(),
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

        var moduleConditionHandler = new Mock<IModuleConditionHandler>();
        moduleConditionHandler
            .Setup(x => x.ShouldIgnore(module, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null));
        var pipeline = new ModuleExecutionPipeline(
            resultRepository.Object,
            engineCancellationToken,
            directHookInvoker.Object,
            moduleConditionHandler.Object,
            OptionsFactory.Create(new PipelineOptions()));

        if (executionStarted is null)
        {
            engineCancellationToken.CancelWithException(new InvalidOperationException("Prior module failure"));
        }

        var executionTask = pipeline.ExecuteAsync(
            module,
            executionContext,
            moduleContext.Object,
            CancellationToken.None);

        if (executionStarted is not null)
        {
            try
            {
                await executionStarted.WaitAsync(TimeSpan.FromSeconds(5));
                engineCancellationToken.CancelWithException(new InvalidOperationException("Prior module failure"));
            }
            finally
            {
                releaseExecution?.Invoke();
            }
        }

        return await executionTask;
    }
}
