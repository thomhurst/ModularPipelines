using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class CoordinatorCancellationResultPipelineTests
{
    [Test]
    [Timeout(30_000)]
    public async Task Failure_Cancelling_A_Module_Running_On_The_Coordinator_Surfaces_The_Failure(
        CancellationToken cancellationToken)
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options =>
        {
            options.TotalInstances = 1;
            options.MaxParallelism = 2;
            options.ModuleResultTimeout = TimeSpan.FromSeconds(20);
        });
        builder.Services.AddSingleton<SlowModuleStarted>();
        builder.AddModule<SlowToStopModule>();
        builder.AddModule<FailingModule>();

        await using var pipeline = await builder.BuildAsync();

        Exception? thrown = null;
        try
        {
            await pipeline.RunAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            thrown = exception;
        }

        // The coordinator's result collector stops waiting and records a cancelled result before
        // the module's local execution finishes stopping. The pipeline must still report the
        // genuine failure rather than a conflicting-result contract violation.
        await Assert.That(thrown).IsNotNull();
        await Assert.That(thrown).IsNotTypeOf<InvalidOperationException>();
        await Assert.That(thrown is ModuleFailedException or PipelineFailedException).IsTrue();

        var resultRegistry = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
        var failingResult = resultRegistry.GetResult(typeof(FailingModule));
        await Assert.That(failingResult).IsNotNull();
        await Assert.That(failingResult!.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(ContainsSimulatedFailure(failingResult.ExceptionOrDefault)).IsTrue();
        await Assert.That(ContainsSimulatedFailure(thrown)).IsTrue();

        var slowModule = pipeline.Services.GetServices<IModule>().OfType<SlowToStopModule>().Single();
        var resultTask = slowModule.AsInternal().ResultTask;
        var registeredResult = resultRegistry.GetResult(typeof(SlowToStopModule));
        await Assert.That(resultTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(registeredResult).IsSameReferenceAs(resultTask.Result);
        await Assert.That(registeredResult!.Status).IsEqualTo(ModuleStatus.Cancelled);
    }

    private static bool ContainsSimulatedFailure(Exception? exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is InvalidDataException { Message: SimulatedFailureMessage })
            {
                return true;
            }
        }

        return false;
    }

    private const string SimulatedFailureMessage = "Simulated worker module failure";

    private sealed class SlowModuleStarted
    {
        private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task Started => _started.Task;

        public void Signal() => _started.TrySetResult();
    }

    private sealed class SlowToStopModule(SlowModuleStarted started) : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            started.Signal();
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Finish stopping after the coordinator has already given up collecting a result.
                await Task.Delay(TimeSpan.FromMilliseconds(500), CancellationToken.None);
                throw;
            }

            return true;
        }
    }

    private sealed class FailingModule(SlowModuleStarted slowModuleStarted) : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Fail only once the slow module is running locally, so cancellation interrupts it.
            await slowModuleStarted.Started.WaitAsync(cancellationToken);
            throw new InvalidDataException(SimulatedFailureMessage);
        }
    }
}
