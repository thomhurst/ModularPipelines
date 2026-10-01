namespace ModularPipelines.Distributed;

/// <summary>
/// Defines the worker-side coordination operations used during distributed pipeline execution.
/// </summary>
/// <remarks>
/// <para>
/// <b>Evolution policy.</b> Coordinator and store interfaces evolve through default interface
/// members: a member added in a 4.x minor release ships with a default implementation, so existing
/// implementations keep compiling and working. Members are only removed or changed in a major
/// release. Implementations that hold connections should also implement <see cref="IAsyncDisposable"/>;
/// the pipeline disposes the coordinator it created when the host shuts down.
/// </para>
/// <para>
/// Every implementation must satisfy the shared coordinator contract tests
/// (<c>DistributedCoordinatorContract</c>).
/// </para>
/// </remarks>
public interface IDistributedWorkerCoordinator
{
    /// <summary>
    /// Registers a worker and its capabilities with the master.
    /// </summary>
    /// <remarks>
    /// Registering the same <see cref="WorkerRegistration.WorkerId"/> and
    /// <see cref="WorkerRegistration.RegisteredAt"/> again is a reconnect and succeeds. Registering a
    /// worker identifier that is live under a different <see cref="WorkerRegistration.RegisteredAt"/>
    /// throws <see cref="InvalidOperationException"/> instead of replacing the live worker.
    /// </remarks>
    Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken);

    /// <summary>
    /// Waits for and claims the next assignment the worker can execute.
    /// </summary>
    /// <remarks>
    /// The returned lease belongs to <paramref name="workerId"/> and stays valid while that worker's
    /// heartbeats list the module in <see cref="WorkerStatus.InFlightModules"/>. After a
    /// <see cref="DistributedCancellationReason.PipelineFailed"/> broadcast only AlwaysRun assignments
    /// are returned; after a <see cref="DistributedCancellationReason.Stopped"/> broadcast none are.
    /// </remarks>
    /// <param name="workerId">The claiming worker.</param>
    /// <param name="workerCapabilities">The capabilities the claiming worker offers.</param>
    /// <param name="cancellationToken">Cancels the wait.</param>
    /// <returns>The claimed lease, or <see langword="null"/> once the master has signalled completion.</returns>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was canceled.</exception>
    Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken);

    /// <summary>
    /// Publishes a module result and releases the lease under which it was produced.
    /// </summary>
    /// <remarks>
    /// The first result published for a module is final. Later results for the same module, for
    /// example from a lease that expired and was executed again, are ignored.
    /// </remarks>
    /// <param name="result">The result to publish.</param>
    /// <param name="lease">The lease the result completes, or <see langword="null"/> for a result the master publishes without a claim.</param>
    /// <param name="cancellationToken">Cancels the publication.</param>
    Task PublishResultAsync(
        SerializedModuleResult result,
        ModuleLease? lease,
        CancellationToken cancellationToken);

    /// <summary>
    /// Waits for the final result of a module.
    /// </summary>
    Task<SerializedModuleResult> WaitForResultAsync(
        ModuleId moduleId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Reports that a worker is alive and renews the leases on its <see cref="WorkerStatus.InFlightModules"/>.
    /// </summary>
    Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken);

    /// <summary>
    /// Waits until the master broadcasts distributed cancellation.
    /// </summary>
    /// <returns>Why the master canceled execution.</returns>
    Task<DistributedCancellationReason> WaitForCancellationAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether the master stopped without signalling completion, for example because its
    /// process exited, failed or crashed, or because the worker can no longer reach it.
    /// </summary>
    /// <remarks>
    /// Workers poll this every <see cref="DistributedOptions.WorkerHeartbeatInterval"/>. When it
    /// returns <see langword="true"/> the worker cancels its in-flight modules, including AlwaysRun
    /// modules, and fails, because no master remains to collect their results. Once the master has
    /// signalled completion this returns <see langword="false"/>, even after the master exits. The
    /// default implementation cannot observe the master and always returns <see langword="false"/>.
    /// </remarks>
    /// <returns><see langword="true"/> when the master is gone without having signalled completion.</returns>
    Task<bool> IsMasterLostAsync(CancellationToken cancellationToken) => Task.FromResult(false);
}
