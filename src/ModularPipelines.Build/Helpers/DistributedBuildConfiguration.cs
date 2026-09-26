using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Extensions;
using StackExchange.Redis;

namespace ModularPipelines.Build.Helpers;

internal static class DistributedBuildConfiguration
{
    public const string MasterCapability = "ci-master";

    public static async Task PrintRedisDiagnosticsAsync(Func<string, string?> getEnvironmentVariable, ILogger logger)
    {
        var connectionOptions = ConfigurationOptions.Parse(getEnvironmentVariable("REDIS_ENDPOINT")
            ?? throw new InvalidOperationException("REDIS_ENDPOINT is required for Redis diagnostics."));
        connectionOptions.Password = getEnvironmentVariable("REDIS_KEY");
        connectionOptions.Ssl = true;
        connectionOptions.AbortOnConnectFail = true;
        connectionOptions.ConnectRetry = 0;
        connectionOptions.ConnectTimeout = 5000;
        connectionOptions.AsyncTimeout = 5000;
        try
        {
            using var connection = await ConnectionMultiplexer.ConnectAsync(connectionOptions).ConfigureAwait(false);
            var server = connection.GetServer(connection.GetEndPoints()[0]);
            foreach (var section in new[] { "memory", "stats" })
            {
                var information = await server.InfoAsync(section).ConfigureAwait(false);
                foreach (var entry in information.SelectMany(static group => group))
                {
                    // Never print endpoint, credentials, or arbitrary server fields.
                    if (entry.Key is "used_memory" or "maxmemory" or "evicted_keys"
                        && long.TryParse(entry.Value, out var value))
                    {
                        logger.LogInformation("Redis {Counter}: {Value}", entry.Key, value);
                    }
                }
            }
        }
        catch (Exception exception) when (exception is RedisException or TimeoutException)
        {
            logger.LogWarning("Redis capacity diagnostics unavailable; the service may restrict INFO.");
        }
    }

    public static bool Configure(PipelineBuilder builder, Func<string, string?> getEnvironmentVariable)
    {
        var indexText = getEnvironmentVariable("MODULARPIPELINES_INSTANCE_INDEX");
        var countText = getEnvironmentVariable("MODULARPIPELINES_TOTAL_INSTANCES");
        if (string.IsNullOrWhiteSpace(indexText) && string.IsNullOrWhiteSpace(countText))
        {
            return true;
        }

        var index = ParseInteger("MODULARPIPELINES_INSTANCE_INDEX", indexText, 0);
        var count = ParseInteger("MODULARPIPELINES_TOTAL_INSTANCES", countText, 1);
        if (index >= count)
        {
            throw new InvalidOperationException("MODULARPIPELINES_INSTANCE_INDEX must be less than MODULARPIPELINES_TOTAL_INSTANCES.");
        }

        var endpoint = getEnvironmentVariable("REDIS_ENDPOINT");
        var password = getEnvironmentVariable("REDIS_KEY");
        if (count == 1 || string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(password))
        {
            // Secretless matrix jobs must not each repeat the standalone pipeline.
            return index == 0;
        }

        var runId = getEnvironmentVariable("MODULARPIPELINES_RUN_ID");
        if (string.IsNullOrWhiteSpace(runId))
        {
            var githubRunId = getEnvironmentVariable("GITHUB_RUN_ID");
            var githubRunAttempt = getEnvironmentVariable("GITHUB_RUN_ATTEMPT");
            if (string.IsNullOrWhiteSpace(githubRunId) || string.IsNullOrWhiteSpace(githubRunAttempt))
            {
                throw new InvalidOperationException("Distributed builds require MODULARPIPELINES_RUN_ID or both GITHUB_RUN_ID and GITHUB_RUN_ATTEMPT.");
            }

            runId = $"{githubRunId}-{githubRunAttempt}";
        }

        var connection = ConfigurationOptions.Parse(endpoint);
        connection.Password = password;
        connection.Ssl = true;
        connection.AbortOnConnectFail = false;

        // Keep credentials as typed options: connection-string serialization does not
        // escape commas in passwords. Both Redis services share this one connection.
        builder.Services.AddSingleton(connection);
        builder.Services.AddSingleton<IConnectionMultiplexer>(services =>
            ConnectionMultiplexer.Connect(services.GetRequiredService<ConfigurationOptions>()));

        builder.AddDistributedMode(options =>
        {
            options.InstanceIndex = index;
            options.TotalInstances = count;
            options.RunId = runId;
            options.Capabilities = index == 0 ? [new Capability(MasterCapability)] : [];
            options.MinimumWorkerCount = count - 1;
            options.CapabilityTimeout = TimeSpan.FromMinutes(10);
            // The macOS compilation alone can take more than 50 minutes. Keep result
            // failure bounded below the workflow's 90-minute lifetime.
            options.ModuleResultTimeout = TimeSpan.FromMinutes(65);
            options.MaxParallelism = 2;
        });
        builder.AddRedisDistributed(options =>
        {
            options.KeyPrefix = "modularpipelines-ci";
            options.KeyExpiration = TimeSpan.FromHours(2);
        }, options => options.TimeToLive = TimeSpan.FromHours(2));
        return true;
    }

    private static int ParseInteger(string name, string? value, int minimum)
    {
        if (!int.TryParse(value, out var result) || result < minimum)
        {
            throw new InvalidOperationException($"{name} must be an integer greater than or equal to {minimum}.");
        }

        return result;
    }
}
