using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.SignalR;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;
using ModularPipelines.Distributed.SignalR.Server;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

/// <summary>
/// Integration tests that start a real SignalR server and connect a client to it.
/// Validates the full serialization round-trip for hub method invocations.
/// </summary>
public class SignalRIntegrationTests
{
    private static readonly TimeSpan[] ReconnectDelays =
    [
        TimeSpan.Zero,
        TimeSpan.FromMilliseconds(100),
        TimeSpan.FromMilliseconds(250),
        TimeSpan.FromMilliseconds(500),
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
    ];

    private sealed class GatedRetryPolicy(ManualResetEventSlim reconnectAllowed, CancellationToken cancellationToken) : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            // Do not reconnect to the old server while its WebSockets are draining.
            reconnectAllowed.Wait(cancellationToken);
            return retryContext.PreviousRetryCount < ReconnectDelays.Length
                ? ReconnectDelays[retryContext.PreviousRetryCount]
                : null;
        }
    }

    private static HubConnection BuildClient(
        string serverUrl,
        string hubPath,
        bool enableImmediateReconnect = false,
        IRetryPolicy? retryPolicy = null)
    {
        var builder = new HubConnectionBuilder()
            .WithUrl($"{serverUrl}{hubPath}")
            .AddJsonProtocol(jsonOptions =>
            {
                jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
                jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        if (retryPolicy is not null)
        {
            builder.WithAutomaticReconnect(retryPolicy);
        }
        else if (enableImmediateReconnect)
        {
            builder.WithAutomaticReconnect(ReconnectDelays);
        }

        return builder.Build();
    }
    [Test]
    public async Task RegisterWorker_RoundTrip_Succeeds()
    {
        // Arrange — start a real SignalR server
        var options = new SignalRDistributedOptions
        {
            MasterUrl = "http://127.0.0.1:0", // OS picks a free port
        };
        var masterState = new SignalRMasterState();
        var serverHost = new MasterServerHost();

        try
        {
            await serverHost.StartAsync(options, masterState, NullLoggerFactory.Instance, CancellationToken.None);

            var connection = BuildClient(serverHost.AdvertisedUrl, options.HubPath);
            await connection.StartAsync();

            // Act — invoke RegisterWorker with a WorkerRegistration containing HashSet<Capability>
            var registration = new WorkerRegistration(
                WorkerIndex: 1,
                Capabilities: ["linux", "x64"],
                RegisteredAt: DateTimeOffset.UtcNow);

            await connection.InvokeAsync(HubMethodNames.RegisterWorker, registration, null);

            // Assert — master state should have the worker
            await Assert.That(masterState.Registrations.Count).IsEqualTo(1);
            await Assert.That(masterState.Registrations[1].WorkerIndex).IsEqualTo(1);
            await Assert.That(masterState.Registrations[1].Capabilities).Contains("linux");
            await Assert.That(masterState.Registrations[1].Capabilities).Contains("x64");

            await connection.DisposeAsync();
        }
        finally
        {
            await serverHost.DisposeAsync();
        }
    }

    [Test]
    public async Task RegisterWorker_Replays_Pending_Cancellation()
    {
        var options = new SignalRDistributedOptions
        {
            MasterUrl = "http://127.0.0.1:0",
        };
        var masterState = new SignalRMasterState();
        masterState.CancellationRequested.SetResult();
        var serverHost = new MasterServerHost();

        try
        {
            await serverHost.StartAsync(
                options,
                masterState,
                NullLoggerFactory.Instance,
                CancellationToken.None);

            var cancellationReceived = new TaskCompletionSource(
                TaskCreationOptions.RunContinuationsAsynchronously);
            await using var connection = BuildClient(serverHost.AdvertisedUrl, options.HubPath);
            connection.On(
                HubMethodNames.BroadcastCancellation,
                () => cancellationReceived.TrySetResult());
            await connection.StartAsync();

            await connection.InvokeAsync(
                HubMethodNames.RegisterWorker,
                new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
                null);

            await cancellationReceived.Task.WaitAsync(TimeSpan.FromSeconds(5));
        }
        finally
        {
            await serverHost.DisposeAsync();
        }
    }

    [Test]
    public async Task PublishResult_RoundTrip_Succeeds()
    {
        var options = new SignalRDistributedOptions
        {
            MasterUrl = "http://127.0.0.1:0",
        };
        var masterState = new SignalRMasterState();
        var serverHost = new MasterServerHost();

        try
        {
            await serverHost.StartAsync(options, masterState, NullLoggerFactory.Instance, CancellationToken.None);

            var connection = BuildClient(serverHost.AdvertisedUrl, options.HubPath);
            await connection.StartAsync();

            // Register first (required by hub)
            await connection.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
                null);

            // Pre-create a result waiter
            var tcs = new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
            masterState.ResultWaiters["TestModule"] = tcs;
            AssignWorker(masterState, 1, "TestModule");

            // Act — publish a result
            var result = new SerializedModuleResult(
                ModuleTypeName: "TestModule",
                ResultTypeName: "System.String",
                WorkerIndex: 1,
                Payload: "{\"Value\":\"hello\"}",
                CompletedAt: DateTimeOffset.UtcNow);

            await connection.InvokeAsync(HubMethodNames.PublishResult, result);

            // Assert — the TCS should be completed
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var collected = await tcs.Task.WaitAsync(cts.Token);
            await Assert.That(collected.ModuleTypeName).IsEqualTo("TestModule");
            await Assert.That(collected.Payload).IsEqualTo("{\"Value\":\"hello\"}");

            await connection.DisposeAsync();
        }
        finally
        {
            await serverHost.DisposeAsync();
        }
    }

    [Test]
    public async Task ModuleAssignment_RoundTrip_Succeeds()
    {
        var options = new SignalRDistributedOptions
        {
            MasterUrl = "http://127.0.0.1:0",
        };
        var masterState = new SignalRMasterState();
        var serverHost = new MasterServerHost();

        try
        {
            await serverHost.StartAsync(options, masterState, NullLoggerFactory.Instance, CancellationToken.None);

            var connection = BuildClient(serverHost.AdvertisedUrl, options.HubPath);

            // Track received assignments
            ModuleAssignment? receivedAssignment = null;
            var assignmentReceived = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            connection.On<ModuleAssignment>(HubMethodNames.ReceiveAssignment, assignment =>
            {
                receivedAssignment = assignment;
                assignmentReceived.TrySetResult();
            });

            await connection.StartAsync();

            // Register as idle worker
            await connection.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(1, ["linux"], DateTimeOffset.UtcNow),
                null);

            // Enqueue work via master state (simulating master coordinator)
            var moduleAssignment = new ModuleAssignment(
                ModuleTypeName: "MyModule",
                ResultTypeName: "System.Int32",
                RequiredCapabilities: [],
                AssignedAt: DateTimeOffset.UtcNow,
                Configuration: new ModuleAssignmentOptions(null, false));

            masterState.PendingAssignments.Enqueue(moduleAssignment);

            // Request work — triggers server to dequeue and push assignment
            await connection.InvokeAsync(HubMethodNames.RequestWork,
                new HashSet<Capability> { "linux" });

            // Wait for assignment to arrive
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await assignmentReceived.Task.WaitAsync(cts.Token);

            // Assert
            await Assert.That(receivedAssignment).IsNotNull();
            await Assert.That(receivedAssignment!.ModuleTypeName).IsEqualTo("MyModule");
            await Assert.That(receivedAssignment.ResultTypeName).IsEqualTo("System.Int32");

            await connection.DisposeAsync();
        }
        finally
        {
            await serverHost.DisposeAsync();
        }
    }

    [Test]
    public async Task MultiWorker_CapabilityRouting_RoutesToCorrectWorker()
    {
        // Arrange: 3 workers with different capabilities
        // Worker 1: linux
        // Worker 2: windows
        // Worker 3: linux + docker
        // Enqueue: a linux-only module, a windows module, and a docker module
        // Assert: each module goes to the right worker

        var options = new SignalRDistributedOptions { MasterUrl = "http://127.0.0.1:0" };
        var masterState = new SignalRMasterState();
        var serverHost = new MasterServerHost();

        try
        {
            await serverHost.StartAsync(options, masterState, NullLoggerFactory.Instance, CancellationToken.None);
            var serverUrl = serverHost.AdvertisedUrl;

            // Create 3 worker connections
            var worker1 = BuildClient(serverUrl, options.HubPath);
            var worker2 = BuildClient(serverUrl, options.HubPath);
            var worker3 = BuildClient(serverUrl, options.HubPath);

            // Track which assignments each worker receives
            var worker1Assignments = new ConcurrentBag<ModuleAssignment>();
            var worker2Assignments = new ConcurrentBag<ModuleAssignment>();
            var worker3Assignments = new ConcurrentBag<ModuleAssignment>();
            var allAssigned = new CountdownEvent(3);

            worker1.On<ModuleAssignment>(HubMethodNames.ReceiveAssignment, a =>
            {
                worker1Assignments.Add(a);
                allAssigned.Signal();
            });
            worker2.On<ModuleAssignment>(HubMethodNames.ReceiveAssignment, a =>
            {
                worker2Assignments.Add(a);
                allAssigned.Signal();
            });
            worker3.On<ModuleAssignment>(HubMethodNames.ReceiveAssignment, a =>
            {
                worker3Assignments.Add(a);
                allAssigned.Signal();
            });

            // Connect and register all workers
            await Task.WhenAll(worker1.StartAsync(), worker2.StartAsync(), worker3.StartAsync());

            await worker1.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(1, ["linux"], DateTimeOffset.UtcNow),
                null);
            await worker2.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(2, ["windows"], DateTimeOffset.UtcNow),
                null);
            await worker3.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(3, ["linux", "docker"], DateTimeOffset.UtcNow),
                null);

            // Enqueue 3 modules with different capability requirements
            var windowsModule = new ModuleAssignment(
                "WindowsBuildModule", "System.String",
                ["windows"],
                DateTimeOffset.UtcNow,
                new ModuleAssignmentOptions(null, false));

            var dockerModule = new ModuleAssignment(
                "DockerBuildModule", "System.String",
                ["linux", "docker"],
                DateTimeOffset.UtcNow,
                new ModuleAssignmentOptions(null, false));

            var genericModule = new ModuleAssignment(
                "GenericModule", "System.String",
                [],
                DateTimeOffset.UtcNow,
                new ModuleAssignmentOptions(null, false));

            masterState.PendingAssignments.Enqueue(windowsModule);
            masterState.PendingAssignments.Enqueue(dockerModule);
            masterState.PendingAssignments.Enqueue(genericModule);

            // All workers request work
            await worker1.InvokeAsync(HubMethodNames.RequestWork, new HashSet<Capability> { "linux" });
            await worker2.InvokeAsync(HubMethodNames.RequestWork, new HashSet<Capability> { "windows" });
            await worker3.InvokeAsync(HubMethodNames.RequestWork, new HashSet<Capability> { "linux", "docker" });

            // Wait for all assignments to be distributed
            allAssigned.Wait(TimeSpan.FromSeconds(5));

            // Assert: windows module went to worker 2 (only one with "windows")
            await Assert.That(worker2Assignments.Any(a => a.ModuleTypeName == "WindowsBuildModule")).IsTrue();

            // Assert: docker module went to worker 3 (only one with "linux" + "docker")
            await Assert.That(worker3Assignments.Any(a => a.ModuleTypeName == "DockerBuildModule")).IsTrue();

            // Assert: generic module (no requirements) went to worker 1 (first idle worker to request)
            await Assert.That(worker1Assignments.Any(a => a.ModuleTypeName == "GenericModule")).IsTrue();

            await Task.WhenAll(
                worker1.DisposeAsync().AsTask(),
                worker2.DisposeAsync().AsTask(),
                worker3.DisposeAsync().AsTask());
        }
        finally
        {
            await serverHost.DisposeAsync();
        }
    }

    [Test]
    public async Task Worker_Fetches_Dependency_Result_After_It_Is_Published()
    {
        var options = new SignalRDistributedOptions { MasterUrl = "http://127.0.0.1:0" };
        var masterState = new SignalRMasterState();
        var serverHost = new MasterServerHost();

        try
        {
            await serverHost.StartAsync(options, masterState, NullLoggerFactory.Instance, CancellationToken.None);
            var serverUrl = serverHost.AdvertisedUrl;

            var worker1 = BuildClient(serverUrl, options.HubPath);
            var worker2 = BuildClient(serverUrl, options.HubPath);

            await Task.WhenAll(worker1.StartAsync(), worker2.StartAsync());

            await worker1.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
                null);
            await worker2.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(2, [], DateTimeOffset.UtcNow),
                null);

            // Pre-create result waiter for the master side
            masterState.ResultWaiters["BuildModule"] = new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
            AssignWorker(masterState, 1, "BuildModule");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var resultTask = worker2.InvokeAsync<SerializedModuleResult>(
                HubMethodNames.WaitForResult,
                "BuildModule",
                cts.Token);

            var result = new SerializedModuleResult(
                "BuildModule", "System.String", 1, "{\"Output\":\"build.zip\"}", DateTimeOffset.UtcNow);
            await worker1.InvokeAsync(HubMethodNames.PublishResult, result);

            var fetchedResult = await resultTask;
            await Assert.That(fetchedResult.ModuleTypeName).IsEqualTo("BuildModule");
            await Assert.That(fetchedResult.Payload).IsEqualTo("{\"Output\":\"build.zip\"}");

            // Master should also have the result
            var masterResult = await masterState.ResultWaiters["BuildModule"].Task;
            await Assert.That(masterResult.ModuleTypeName).IsEqualTo("BuildModule");

            await Task.WhenAll(
                worker1.DisposeAsync().AsTask(),
                worker2.DisposeAsync().AsTask());
        }
        finally
        {
            await serverHost.DisposeAsync();
        }
    }

    [Test]
    [Timeout(30_000)]
    public async Task Rejected_Result_Preserves_Assignment_For_Reregistration_And_Resubmission(
        CancellationToken cancellationToken)
    {
        var options = new SignalRDistributedOptions { MasterUrl = "http://127.0.0.1:0" };
        var state = new SignalRMasterState();
        await using var serverHost = new MasterServerHost();
        await serverHost.StartAsync(options, state, NullLoggerFactory.Instance, cancellationToken);
        await using var original = BuildClient(serverHost.AdvertisedUrl, options.HubPath);
        await using var replacement = BuildClient(serverHost.AdvertisedUrl, options.HubPath);
        var coordinator = new SignalRWorkerCoordinator(original, NullLogger<SignalRWorkerCoordinator>.Instance);
        var deliveries = 0;
        var firstDelivery = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var deliverySubscription = original.On<ModuleAssignment>(HubMethodNames.ReceiveAssignment, _ =>
        {
            Interlocked.Increment(ref deliveries);
            firstDelivery.TrySetResult();
        });
        var registration = new WorkerRegistration(1, [], DateTimeOffset.UtcNow);
        var assignment = new ModuleAssignment("CompletedModule", "System.String", [],
            DateTimeOffset.UtcNow, new ModuleAssignmentOptions(null, false));
        var waiter = new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        state.ResultWaiters[assignment.ModuleTypeName] = waiter;
        state.PendingAssignments.Enqueue(assignment);

        await original.StartAsync(cancellationToken);
        await coordinator.RegisterWorkerAsync(registration, cancellationToken);
        var received = await coordinator.DequeueModuleAsync(new HashSet<Capability>(), cancellationToken);
        await firstDelivery.Task.WaitAsync(cancellationToken);
        await Assert.That(received!.ModuleTypeName).IsEqualTo(assignment.ModuleTypeName);
        await replacement.StartAsync(cancellationToken);
        await replacement.InvokeAsync(HubMethodNames.RegisterWorker, registration,
            assignment.ModuleTypeName, cancellationToken);

        var result = new SerializedModuleResult(assignment.ModuleTypeName, assignment.ResultTypeName,
            1, "{}", DateTimeOffset.UtcNow);
        await Assert.That(() => coordinator.PublishResultAsync(result, cancellationToken))
            .Throws<Microsoft.AspNetCore.SignalR.HubException>();
        await Assert.That(waiter.Task.IsCompleted).IsFalse();

        // Registration uses the coordinator's retained in-flight assignment to reclaim
        // the completed execution, without receiving or executing another assignment.
        await coordinator.RegisterWorkerAsync(registration, cancellationToken);
        await Assert.That(state.Workers[original.ConnectionId!].CurrentAssignment).IsSameReferenceAs(assignment);
        await coordinator.PublishResultAsync(result, cancellationToken);
        await Assert.That(await waiter.Task.WaitAsync(cancellationToken)).IsEqualTo(result);
        await Assert.That(deliveries).IsEqualTo(1);
        await Assert.That(state.PendingAssignments).IsEmpty();
        await Assert.That(state.GetPendingReconnect(1)).IsNull();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Worker_Result_Publication_Survives_Disconnect_And_Reconnect(CancellationToken cancellationToken)
    {
        var options = new SignalRDistributedOptions { MasterUrl = "http://127.0.0.1:0" };
        var state = new SignalRMasterState();
        MasterServerHost? serverHost = new();
        using var reconnectAllowed = new ManualResetEventSlim();
        try
        {
            await serverHost.StartAsync(options, state, NullLoggerFactory.Instance, cancellationToken);
            var serverUrl = serverHost.AdvertisedUrl;
            await using var connection = BuildClient(serverUrl, options.HubPath, retryPolicy: new GatedRetryPolicy(reconnectAllowed, cancellationToken));
            var coordinator = new SignalRWorkerCoordinator(connection, NullLogger<SignalRWorkerCoordinator>.Instance);
            var reconnecting = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            connection.Reconnecting += _ =>
            {
                reconnecting.TrySetResult();
                return Task.CompletedTask;
            };
            var deliveries = 0;
            using var subscription = connection.On<ModuleAssignment>(HubMethodNames.ReceiveAssignment,
                _ => Interlocked.Increment(ref deliveries));
            var assignment = new ModuleAssignment("CompletedModule", "System.String", [],
                DateTimeOffset.UtcNow, new ModuleAssignmentOptions(null, false));
            var waiter = new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            state.ResultWaiters[assignment.ModuleTypeName] = waiter;
            state.PendingAssignments.Enqueue(assignment);
            await connection.StartAsync(cancellationToken);
            await coordinator.RegisterWorkerAsync(new WorkerRegistration(1, [], DateTimeOffset.UtcNow), cancellationToken);
            await coordinator.DequeueModuleAsync(new HashSet<Capability>(), cancellationToken);

            await serverHost.DisposeAsync();
            serverHost = null;
            reconnectAllowed.Set();
            await reconnecting.Task.WaitAsync(cancellationToken);
            var expected = new SerializedModuleResult(assignment.ModuleTypeName, assignment.ResultTypeName,
                1, "{\"Output\":\"completed-successfully\"}", DateTimeOffset.UtcNow);
            var publication = coordinator.PublishResultAsync(expected, cancellationToken);

            options.MasterUrl = serverUrl;
            serverHost = new MasterServerHost();
            await serverHost.StartAsync(options, state, NullLoggerFactory.Instance, cancellationToken);
            await publication.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);

            await Assert.That(await waiter.Task.WaitAsync(cancellationToken)).IsEqualTo(expected);
            await Assert.That(deliveries).IsEqualTo(1);
            await Assert.That(state.PendingAssignments).IsEmpty();
            await Assert.That(state.GetPendingReconnect(1)).IsNull();
        }
        finally
        {
            reconnectAllowed.Set();
            if (serverHost is not null)
            {
                await serverHost.DisposeAsync();
            }
        }
    }

    [Test]
    public async Task Worker_Result_Wait_Survives_Disconnect_And_Reconnect()
    {
        var options = new SignalRDistributedOptions { MasterUrl = "http://127.0.0.1:0" };
        var masterState = new SignalRMasterState();
        MasterServerHost? serverHost = new();

        try
        {
            await serverHost.StartAsync(
                options,
                masterState,
                NullLoggerFactory.Instance,
                CancellationToken.None);
            // Bound graceful shutdown while the result invocation is intentionally pending.
            var app = (WebApplication) typeof(MasterServerHost)
                .GetField("_app", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(serverHost)!;
            app.Services.GetRequiredService<IOptions<HostOptions>>().Value.ShutdownTimeout = TimeSpan.FromSeconds(1);
            var serverUrl = serverHost.AdvertisedUrl;
            await using var connection = BuildClient(
                serverUrl,
                options.HubPath,
                enableImmediateReconnect: true);
            var coordinator = new SignalRWorkerCoordinator(
                connection,
                NullLogger<SignalRWorkerCoordinator>.Instance);
            var reconnected = new TaskCompletionSource(
                TaskCreationOptions.RunContinuationsAsynchronously);
            connection.Reconnected += _ =>
            {
                reconnected.TrySetResult();
                return Task.CompletedTask;
            };

            await connection.StartAsync();
            await coordinator.RegisterWorkerAsync(
                new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
                CancellationToken.None);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            var resultTask = coordinator.WaitForResultAsync("BuildModule", cts.Token);
            while (!masterState.ResultWaiters.ContainsKey("BuildModule"))
            {
                await Task.Delay(10, cts.Token);
            }

            await serverHost.DisposeAsync();
            serverHost = null;

            options.MasterUrl = serverUrl;
            serverHost = new MasterServerHost();
            await serverHost.StartAsync(
                options,
                masterState,
                NullLoggerFactory.Instance,
                cts.Token);
            await reconnected.Task.WaitAsync(cts.Token);

            var expected = new SerializedModuleResult(
                "BuildModule",
                "System.String",
                2,
                "{\"Output\":\"build.zip\"}",
                DateTimeOffset.UtcNow);
            masterState.ResultWaiters["BuildModule"].TrySetResult(expected);

            var actual = await resultTask;
            await Assert.That(actual).IsEqualTo(expected);
        }
        finally
        {
            if (serverHost is not null)
            {
                await serverHost.DisposeAsync();
            }
        }
    }

    [Test]
    public async Task FullWorkflow_EnqueueExecutePublish_CompletesSuccessfully()
    {
        // End-to-end test: enqueue work, worker picks it up, publishes result,
        // master collects the result, and completion is signalled

        var options = new SignalRDistributedOptions { MasterUrl = "http://127.0.0.1:0" };
        var masterState = new SignalRMasterState();
        var serverHost = new MasterServerHost();

        try
        {
            await serverHost.StartAsync(options, masterState, NullLoggerFactory.Instance, CancellationToken.None);
            var serverUrl = serverHost.AdvertisedUrl;

            var worker = BuildClient(serverUrl, options.HubPath);

            // Worker receives assignments and "executes" them by publishing results
            var completionSignalled = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            worker.On<ModuleAssignment>(HubMethodNames.ReceiveAssignment, async assignment =>
            {
                // Simulate module execution by publishing a result
                var result = new SerializedModuleResult(
                    assignment.ModuleTypeName,
                    assignment.ResultTypeName,
                    1,
                    $"{{\"Result\":\"executed-{assignment.ModuleTypeName}\"}}",
                    DateTimeOffset.UtcNow);
                await worker.InvokeAsync(HubMethodNames.PublishResult, result);
            });
            worker.On(HubMethodNames.SignalCompletion, () => completionSignalled.TrySetResult());

            await worker.StartAsync();
            await worker.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
                null);

            // Set up result waiters and enqueue 3 modules
            var modules = new[] { "ModuleA", "ModuleB", "ModuleC" };
            var resultTasks = new Dictionary<string, TaskCompletionSource<SerializedModuleResult>>();
            foreach (var moduleName in modules)
            {
                var tcs = new TaskCompletionSource<SerializedModuleResult>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
                masterState.ResultWaiters[moduleName] = tcs;
                resultTasks[moduleName] = tcs;

                masterState.PendingAssignments.Enqueue(new ModuleAssignment(
                    moduleName, "System.String", [],
                    DateTimeOffset.UtcNow, new ModuleAssignmentOptions(null, false)));
            }

            // Worker requests work — will get first assignment, then re-request after each publish
            await worker.InvokeAsync(HubMethodNames.RequestWork, new HashSet<Capability>());

            // Wait for all results
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var results = await Task.WhenAll(
                resultTasks["ModuleA"].Task.WaitAsync(cts.Token),
                resultTasks["ModuleB"].Task.WaitAsync(cts.Token),
                resultTasks["ModuleC"].Task.WaitAsync(cts.Token));

            // Assert all results received
            await Assert.That(results.Length).IsEqualTo(3);
            await Assert.That(results.Any(r => r.ModuleTypeName == "ModuleA")).IsTrue();
            await Assert.That(results.Any(r => r.ModuleTypeName == "ModuleB")).IsTrue();
            await Assert.That(results.Any(r => r.ModuleTypeName == "ModuleC")).IsTrue();

            // Signal completion and verify worker receives it
            await serverHost.HubContext.Clients.All.SendCoreAsync(HubMethodNames.SignalCompletion, [], cts.Token);
            await completionSignalled.Task.WaitAsync(cts.Token);

            await worker.DisposeAsync();
        }
        finally
        {
            await serverHost.DisposeAsync();
        }
    }

    private static void AssignWorker(
        SignalRMasterState state,
        int workerIndex,
        string moduleTypeName)
    {
        var worker = state.Workers.Values.Single(instance =>
            instance.Registration.WorkerIndex == workerIndex);
        if (!worker.TryAssign(new ModuleAssignment(
                moduleTypeName,
                "System.String",
                [],
                DateTimeOffset.UtcNow,
                new ModuleAssignmentOptions(null, false))))
        {
            throw new InvalidOperationException($"Worker {workerIndex} already has an assignment.");
        }
    }
}
