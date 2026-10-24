using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Extensions;
using Respire;

namespace ModularPipelines.Build.Helpers;

internal static class DistributedBuildConfiguration
{
    public const string MasterCapability = "ci-master";

    public static async Task PrintRedisDiagnosticsAsync(Func<string, string?> getEnvironmentVariable, ILogger logger)
    {
        var connectionOptions = RespireOptions.Parse(getEnvironmentVariable("REDIS_ENDPOINT")
            ?? throw new InvalidOperationException("REDIS_ENDPOINT is required for Redis diagnostics.")) with
        {
            Password = getEnvironmentVariable("REDIS_KEY"),
            UseTls = true,
            ConnectTimeout = TimeSpan.FromSeconds(5),
            CommandTimeout = TimeSpan.FromSeconds(5),
        };
        try
        {
#pragma warning disable RESP010 // This one-shot startup probe owns its connection; normal pipeline work uses shared clients.
            await using var connection = await RespireClient.ConnectAsync(connectionOptions).ConfigureAwait(false);
#pragma warning restore RESP010
            foreach (var section in new[] { "memory", "stats" })
            {
                var information = await connection.Server.InfoAsync(section).ConfigureAwait(false);
                foreach (var line in information.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                {
                    var separator = line.IndexOf(':', StringComparison.Ordinal);
                    if (separator <= 0)
                    {
                        continue;
                    }

                    // Never print endpoint, credentials, or arbitrary server fields.
                    var counter = line[..separator];
                    if (counter is "used_memory" or "maxmemory" or "evicted_keys"
                        && long.TryParse(line.AsSpan(separator + 1), out var value))
                    {
                        if (logger.IsEnabled(LogLevel.Information))
                        {
                            logger.LogInformation("Redis {Counter}: {Value}", counter, value);
                        }
                    }
                }
            }
        }
        catch (Exception exception) when (exception is RespireException or SocketException or TimeoutException)
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
            // A standalone run is its own master, so it owns master-only modules.
            builder.AddCapabilities(new Capability(MasterCapability));
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
            if (index != 0)
            {
                return false;
            }

            builder.AddCapabilities(new Capability(MasterCapability));
            return true;
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

        builder.AddDistributedMode(options => options with
        {
            InstanceIndex = index,
            TotalInstances = count,
            RunId = runId,
            Capabilities = index == 0 ? [new Capability(MasterCapability)] : [],
            MinimumWorkerCount = count - 1,
            WorkerRegistrationTimeout = TimeSpan.FromMinutes(10),
            // Workers enforce each module's own timeout; the master's backstop adds this slack
            // after a worker claims the module. A lost runner is detected sooner: its lease expires,
            // and a module no live worker can claim fails after WorkerRegistrationTimeout.
            ModuleResultTimeout = TimeSpan.FromMinutes(15),
            MaxParallelism = 2,
        });
        builder.AddRedisDistributed(options => options with
        {
            ConnectionString = endpoint,
            // Keep credentials as typed options: connection-string serialization does not
            // escape commas in passwords.
            ConfigureConnection = connection => connection with
            {
                Password = password,
                UseTls = true,
                // Six runners share the managed Redis link; build-output chunks queued behind each
                // other exceeded a 5-second timeout while downloading. Stay well under the
                // 30-second worker lease so a stalled heartbeat fails before its lease expires.
                CommandTimeout = TimeSpan.FromSeconds(15),
            },
            KeyPrefix = "modularpipelines-ci",
            TimeToLive = TimeSpan.FromHours(2),
            // Runners are scheduled independently and can finish setup more than five minutes
            // apart. Allow a 30-minute startup window within the workflow's 90-minute job limit,
            // while still failing a worker whose master never starts.
            MasterStartTimeout = TimeSpan.FromMinutes(30),
        });
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
