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

            // Get the actual URL the server is listening on
            var serverUrl = serverHost.AdvertisedUrl;

            // Connect a client
            var connection = new HubConnectionBuilder()
                .WithUrl($"{serverUrl}{options.HubPath}")
                .AddJsonProtocol(jsonOptions =>
                {
                    jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
                    jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .Build();

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
            var serverUrl = serverHost.AdvertisedUrl;

            var connection = new HubConnectionBuilder()
                .WithUrl($"{serverUrl}{options.HubPath}")
                .AddJsonProtocol(jsonOptions =>
                {
                    jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
                    jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .Build();

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
            var serverUrl = serverHost.AdvertisedUrl;

            var connection = new HubConnectionBuilder()
                .WithUrl($"{serverUrl}{options.HubPath}")
                .AddJsonProtocol(jsonOptions =>
                {
                    jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
                    jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .Build();

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
}
