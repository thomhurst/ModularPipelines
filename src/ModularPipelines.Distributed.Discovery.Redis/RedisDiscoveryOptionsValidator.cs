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
        var usesRest = options.RestUrl is not null;
        if (options.RestUrl is { } restUrl
            && (!restUrl.IsAbsoluteUri || restUrl.Scheme is not ("http" or "https")))
        {
            failures.Add($"{nameof(RedisDiscoveryOptions.RestUrl)} must be an absolute http or https URL.");
        }

        if (usesRest != !string.IsNullOrWhiteSpace(options.RestToken))
        {
            failures.Add("RestUrl and RestToken must be configured together.");
        }

        if (!usesRest && string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            failures.Add($"{nameof(RedisDiscoveryOptions.ConnectionString)} is required unless RestUrl is configured.");
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
