using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Coordination;
using ModularPipelines.TestHelpers.Distributed;
using StackExchange.Redis;
using TUnit.Core.Exceptions;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

public class RedisDistributedCoordinatorContractTests
{
    private const string ConnectionStringVariable = "MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING";

    [Test]
    public Task Enqueue_And_Dequeue_RoundTrips()
    {
        return RunContractAsync(DistributedCoordinatorContract.EnqueueAndDequeueRoundTripsAsync);
    }

    [Test]
    public Task Publish_Unblocks_Wait_And_RoundTrips_Result()
    {
        return RunContractAsync(DistributedCoordinatorContract.ResultRoundTripsAfterWaitStartsAsync);
    }

    [Test]
    public Task Completion_Signal_Unblocks_Pending_Dequeue()
    {
        return RunContractAsync(DistributedCoordinatorContract.CompletionUnblocksPendingDequeueAsync);
    }

    private static async Task RunContractAsync(Func<IDistributedCoordinator, Task, Task> contract)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new SkipTestException($"Set {ConnectionStringVariable} to run real Redis contract tests.");
        }

        using var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
        var options = new RedisDistributedOptions
        {
            ConnectionString = connectionString,
            KeyExpiration = TimeSpan.FromMinutes(1),
            KeyPrefix = "modpipe-contract",
            RunIdentifier = Guid.NewGuid().ToString("N"),
        };
        var keys = new RedisKeyBuilder(options.KeyPrefix, options.RunIdentifier!);
        var ready = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var coordinator = new RedisDistributedCoordinator(
            connection.GetDatabase(),
            connection.GetSubscriber(),
            keys,
            options,
            () => ready.TrySetResult());

        await contract(coordinator, ready.Task);
    }
}
