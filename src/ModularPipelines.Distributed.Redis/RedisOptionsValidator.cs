using Microsoft.Extensions.Options;
using Respire;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Validates <see cref="RedisOptions"/> at startup. The unnamed instance configures the
/// distributed coordinator and artifact store, so its keys must outlive a module result wait.
/// </summary>
internal sealed class RedisOptionsValidator(string optionsName, IOptions<DistributedOptions> distributedOptions)
    : IValidateOptions<RedisOptions>
{
    public ValidateOptionsResult Validate(string? name, RedisOptions options)
    {
        if (!string.Equals(name ?? string.Empty, optionsName, StringComparison.Ordinal))
        {
            return ValidateOptionsResult.Skip;
        }

        var failures = new List<string>();
        ValidateConnection(options, failures);

        if (string.IsNullOrWhiteSpace(options.KeyPrefix))
        {
            failures.Add($"{nameof(RedisOptions)}.{nameof(RedisOptions.KeyPrefix)} must not be empty.");
        }

        if (options.ChunkSizeBytes <= 0)
        {
            failures.Add($"{nameof(RedisOptions)}.{nameof(RedisOptions.ChunkSizeBytes)} must be positive.");
        }

        if (options.MasterStartTimeout <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(RedisOptions)}.{nameof(RedisOptions.MasterStartTimeout)} must be positive and finite.");
        }

        ValidateKeyLifetime(name, options, failures);

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }

    private static void ValidateConnection(RedisOptions options, List<string> failures)
    {
        try
        {
            if (RedisConnectionProvider.CreateConfiguration(options).Endpoints.Count == 0)
            {
                failures.Add(
                    $"{nameof(RedisOptions)}.{nameof(RedisOptions.ConnectionString)} or " +
                    $"{nameof(RedisOptions.ConfigureConnection)} must supply at least one endpoint after configuration.");
            }
        }
        catch (Exception exception) when (exception is ArgumentException or RespireConfigurationException)
        {
            // Respire wraps malformed connection strings in RespireConfigurationException.
            failures.Add(
                $"{nameof(RedisOptions)}.{nameof(RedisOptions.ConnectionString)} is invalid: {exception.Message}");
        }
    }

    private void ValidateKeyLifetime(string? name, RedisOptions options, List<string> failures)
    {
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
    }
}
