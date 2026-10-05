using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.SignalR;

/// <summary>
/// Validates <see cref="SignalRDistributedOptions"/> at startup.
/// </summary>
internal sealed class SignalRDistributedOptionsValidator : IValidateOptions<SignalRDistributedOptions>
{
    public ValidateOptionsResult Validate(string? name, SignalRDistributedOptions options)
    {
        var failures = new List<string>();
        if (options.ListenUrl is not { IsAbsoluteUri: true } listenUrl
            || listenUrl.Scheme is not ("http" or "https"))
        {
            failures.Add($"{nameof(SignalRDistributedOptions.ListenUrl)} must be an absolute http or https URL.");
        }

        if (options.AdvertisedUrl is { } advertisedUrl
            && (!advertisedUrl.IsAbsoluteUri || advertisedUrl.Scheme is not ("http" or "https")))
        {
            failures.Add($"{nameof(SignalRDistributedOptions.AdvertisedUrl)} must be an absolute http or https URL.");
        }

        if (string.IsNullOrWhiteSpace(options.HubPath) || !options.HubPath.StartsWith('/'))
        {
            failures.Add($"{nameof(SignalRDistributedOptions.HubPath)} must start with '/'.");
        }

        if (options.AccessToken is { } token && string.IsNullOrWhiteSpace(token))
        {
            failures.Add($"{nameof(SignalRDistributedOptions.AccessToken)} cannot be empty when set.");
        }

        if (options.ConnectionTimeout <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(SignalRDistributedOptions.ConnectionTimeout)} must be positive.");
        }

        if (options.MaxReconnectAttempts < 0)
        {
            failures.Add($"{nameof(SignalRDistributedOptions.MaxReconnectAttempts)} cannot be negative.");
        }

        if (options.KeepAliveInterval <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(SignalRDistributedOptions.KeepAliveInterval)} must be positive.");
        }

        if (options.PeerTimeout < options.KeepAliveInterval * 2)
        {
            failures.Add(
                $"{nameof(SignalRDistributedOptions.PeerTimeout)} must be at least twice {nameof(SignalRDistributedOptions.KeepAliveInterval)}.");
        }

        if (options.MaxMessageSizeBytes <= 0)
        {
            failures.Add($"{nameof(SignalRDistributedOptions.MaxMessageSizeBytes)} must be positive.");
        }

        if (options.Tunnel is null)
        {
            failures.Add($"{nameof(SignalRDistributedOptions.Tunnel)} cannot be null.");
        }
        else if (options.Tunnel.StartupTimeout <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(SignalRTunnelOptions)}.{nameof(SignalRTunnelOptions.StartupTimeout)} must be positive.");
        }

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }
}
