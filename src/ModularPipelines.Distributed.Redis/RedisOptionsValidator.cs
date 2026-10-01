using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Validates <see cref="RedisOptions"/> at startup. The unnamed instance configures the
/// distributed coordinator and artifact store, so its keys must outlive a module result wait.
/// </summary>
internal sealed class RedisOptionsValidator(IOptions<DistributedOptions> distributedOptions)
    : IValidateOptions<RedisOptions>
{
    public ValidateOptionsResult Validate(string? name, RedisOptions options)
    {
        var failures = new List<string>();
        if (string.IsNullOrWhiteSpace(options.ConnectionString) && options.ConfigureConnection is null)
        {
            failures.Add(
                $"{nameof(RedisOptions)}.{nameof(RedisOptions.ConnectionString)} is required unless " +
                $"{nameof(RedisOptions.ConfigureConnection)} supplies the endpoints.");
        }

        if (string.IsNullOrWhiteSpace(options.KeyPrefix))
        {
            failures.Add($"{nameof(RedisOptions)}.{nameof(RedisOptions.KeyPrefix)} must not be empty.");
        }

        if (options.ChunkSizeBytes <= 0)
        {
            failures.Add($"{nameof(RedisOptions)}.{nameof(RedisOptions.ChunkSizeBytes)} must be positive.");
        }

        if (options.TimeToLive <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(RedisOptions)}.{nameof(RedisOptions.TimeToLive)} must be positive.");
        }
        else if (string.IsNullOrEmpty(name))
        {
            // Only the unnamed instance backs run-scoped keys; module cache options are named.
            var moduleResultTimeout = distributedOptions.Value.ModuleResultTimeout;
            if (options.TimeToLive <= moduleResultTimeout)
            {
                failures.Add(
                    $"{nameof(RedisOptions)}.{nameof(RedisOptions.TimeToLive)} ({options.TimeToLive}) must exceed " +
                    $"{nameof(DistributedOptions)}.{nameof(DistributedOptions.ModuleResultTimeout)} ({moduleResultTimeout}) " +
                    "so run keys and artifacts do not expire mid-run.");
            }

            var masterTimeout = distributedOptions.Value.MasterTimeout;
            if (options.TimeToLive <= masterTimeout)
            {
                failures.Add(
                    $"{nameof(RedisOptions)}.{nameof(RedisOptions.TimeToLive)} ({options.TimeToLive}) must exceed " +
                    $"{nameof(DistributedOptions)}.{nameof(DistributedOptions.MasterTimeout)} ({masterTimeout}) " +
                    "so workers see a stopped master's heartbeat go stale before it expires.");
            }
        }

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }
}
