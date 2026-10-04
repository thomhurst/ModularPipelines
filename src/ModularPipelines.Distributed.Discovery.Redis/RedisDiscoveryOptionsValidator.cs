using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Validates <see cref="RedisDiscoveryOptions"/> at startup.
/// </summary>
internal sealed class RedisDiscoveryOptionsValidator : IValidateOptions<RedisDiscoveryOptions>
{
    public ValidateOptionsResult Validate(string? name, RedisDiscoveryOptions options)
    {
        var failures = new List<string>();
        var usesRest = !string.IsNullOrWhiteSpace(options.RestUrl);
        if (usesRest != !string.IsNullOrWhiteSpace(options.RestToken))
        {
            failures.Add("RestUrl and RestToken must be configured together.");
        }

        if (!usesRest && string.IsNullOrWhiteSpace(options.ConnectionString) && options.ConfigureConnection is null)
        {
            failures.Add($"{nameof(RedisDiscoveryOptions.ConnectionString)} is required unless ConfigureConnection supplies endpoints or RestUrl and RestToken are configured.");
        }
        else if (!usesRest && options.GetConnectionConfiguration().EndPoints.Count == 0)
        {
            failures.Add("Redis discovery requires at least one TCP endpoint.");
        }

        if (string.IsNullOrWhiteSpace(options.KeyPrefix))
        {
            failures.Add($"{nameof(RedisDiscoveryOptions.KeyPrefix)} cannot be empty.");
        }

        if (options.TimeToLive < TimeSpan.FromSeconds(1))
        {
            failures.Add($"{nameof(RedisDiscoveryOptions.TimeToLive)} must be at least one second.");
        }

        if (options.DiscoveryTimeout <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(RedisDiscoveryOptions.DiscoveryTimeout)} must be positive.");
        }

        if (options.PollInterval <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(RedisDiscoveryOptions.PollInterval)} must be positive.");
        }

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }
}
