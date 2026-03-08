using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.SignalR.Configuration;
using ModularPipelines.Distributed.SignalR.Hub;
using ModularPipelines.Distributed.SignalR.Server;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

/// <summary>
/// Integration tests that start a real SignalR server and connect a client to it.
/// Validates the full serialization round-trip for hub method invocations.
/// </summary>
public class SignalRIntegrationTests
{
    private static HubConnection BuildClient(string serverUrl, string hubPath)
    {
        return new HubConnectionBuilder()
            .WithUrl($"{serverUrl}{hubPath}")
            .AddJsonProtocol(jsonOptions =>
            {
                jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
                jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
            })
            .Build();
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

            // Act — invoke RegisterWorker with a WorkerRegistration containing HashSet<string>
            var registration = new WorkerRegistration(
                WorkerIndex: 1,
                Capabilities: new HashSet<string> { "linux", "x64" },
                RegisteredAt: DateTimeOffset.UtcNow);

            await connection.InvokeAsync(HubMethodNames.RegisterWorker, registration);

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
                new WorkerRegistration(1, new HashSet<string>(), DateTimeOffset.UtcNow));

            // Pre-create a result waiter
            var tcs = new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
            masterState.ResultWaiters["TestModule"] = tcs;

            // Act — publish a result
            var result = new SerializedModuleResult(
                ModuleTypeName: "TestModule",
                ResultTypeName: "System.String",
                WorkerIndex: 1,
                SerializedJson: "{\"Value\":\"hello\"}",
                CompletedAt: DateTimeOffset.UtcNow);

            await connection.InvokeAsync(HubMethodNames.PublishResult, result);

            // Assert — the TCS should be completed
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var collected = await tcs.Task.WaitAsync(cts.Token);
            await Assert.That(collected.ModuleTypeName).IsEqualTo("TestModule");
            await Assert.That(collected.SerializedJson).IsEqualTo("{\"Value\":\"hello\"}");

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
                new WorkerRegistration(1, new HashSet<string> { "linux" }, DateTimeOffset.UtcNow));

            // Enqueue work via master state (simulating master coordinator)
            var moduleAssignment = new ModuleAssignment(
                ModuleTypeName: "MyModule",
                ResultTypeName: "System.Int32",
                RequiredCapabilities: new HashSet<string>(),
                MatrixTarget: null,
                AssignedAt: DateTimeOffset.UtcNow,
                Configuration: new ModuleAssignmentConfig(null, 0, false));

            masterState.PendingAssignments.Enqueue(moduleAssignment);

            // Request work — triggers server to dequeue and push assignment
            await connection.InvokeAsync(HubMethodNames.RequestWork,
                new HashSet<string> { "linux" });

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
                new WorkerRegistration(1, new HashSet<string> { "linux" }, DateTimeOffset.UtcNow));
            await worker2.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(2, new HashSet<string> { "windows" }, DateTimeOffset.UtcNow));
            await worker3.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(3, new HashSet<string> { "linux", "docker" }, DateTimeOffset.UtcNow));

            // Enqueue 3 modules with different capability requirements
            var windowsModule = new ModuleAssignment(
                "WindowsBuildModule", "System.String",
                new HashSet<string> { "windows" },
                null, DateTimeOffset.UtcNow,
                new ModuleAssignmentConfig(null, 0, false));

            var dockerModule = new ModuleAssignment(
                "DockerBuildModule", "System.String",
                new HashSet<string> { "linux", "docker" },
                null, DateTimeOffset.UtcNow,
                new ModuleAssignmentConfig(null, 0, false));

            var genericModule = new ModuleAssignment(
                "GenericModule", "System.String",
                new HashSet<string>(),
                null, DateTimeOffset.UtcNow,
                new ModuleAssignmentConfig(null, 0, false));

            masterState.PendingAssignments.Enqueue(windowsModule);
            masterState.PendingAssignments.Enqueue(dockerModule);
            masterState.PendingAssignments.Enqueue(genericModule);

            // All workers request work
            await worker1.InvokeAsync(HubMethodNames.RequestWork, new HashSet<string> { "linux" });
            await worker2.InvokeAsync(HubMethodNames.RequestWork, new HashSet<string> { "windows" });
            await worker3.InvokeAsync(HubMethodNames.RequestWork, new HashSet<string> { "linux", "docker" });

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
    public async Task MultiWorker_ResultBroadcast_OtherWorkersReceiveDependencyResults()
    {
        // When worker 1 publishes a result, worker 2 should receive it
        // as a dependency result (for CompletionSource pre-population)

        var options = new SignalRDistributedOptions { MasterUrl = "http://127.0.0.1:0" };
        var masterState = new SignalRMasterState();
        var serverHost = new MasterServerHost();

        try
        {
            await serverHost.StartAsync(options, masterState, NullLoggerFactory.Instance, CancellationToken.None);
            var serverUrl = serverHost.AdvertisedUrl;

            var worker1 = BuildClient(serverUrl, options.HubPath);
            var worker2 = BuildClient(serverUrl, options.HubPath);

            // Track dependency results received by worker 2
            SerializedModuleResult? receivedByWorker2 = null;
            var resultReceived = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            worker2.On<SerializedModuleResult>(HubMethodNames.ReceiveDependencyResult, result =>
            {
                receivedByWorker2 = result;
                resultReceived.TrySetResult();
            });

            await Task.WhenAll(worker1.StartAsync(), worker2.StartAsync());

            await worker1.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(1, new HashSet<string>(), DateTimeOffset.UtcNow));
            await worker2.InvokeAsync(HubMethodNames.RegisterWorker,
                new WorkerRegistration(2, new HashSet<string>(), DateTimeOffset.UtcNow));

            // Pre-create result waiter for the master side
            masterState.ResultWaiters["BuildModule"] = new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            // Worker 1 publishes a result
            var result = new SerializedModuleResult(
                "BuildModule", "System.String", 1, "{\"Output\":\"build.zip\"}", DateTimeOffset.UtcNow);
            await worker1.InvokeAsync(HubMethodNames.PublishResult, result);

            // Worker 2 should receive the dependency result
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await resultReceived.Task.WaitAsync(cts.Token);

            await Assert.That(receivedByWorker2).IsNotNull();
            await Assert.That(receivedByWorker2!.ModuleTypeName).IsEqualTo("BuildModule");
            await Assert.That(receivedByWorker2.SerializedJson).IsEqualTo("{\"Output\":\"build.zip\"}");

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
                new WorkerRegistration(1, new HashSet<string>(), DateTimeOffset.UtcNow));

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
                    moduleName, "System.String", new HashSet<string>(),
                    null, DateTimeOffset.UtcNow, new ModuleAssignmentConfig(null, 0, false)));
            }

            // Worker requests work — will get first assignment, then re-request after each publish
            await worker.InvokeAsync(HubMethodNames.RequestWork, new HashSet<string>());

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
            await serverHost.HubContext.Clients.All.SendCoreAsync(HubMethodNames.SignalCompletion, Array.Empty<object?>(), cts.Token);
            await completionSignalled.Task.WaitAsync(cts.Token);

            await worker.DisposeAsync();
        }
        finally
        {
            await serverHost.DisposeAsync();
        }
    }
}
