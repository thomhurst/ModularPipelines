using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "jobs", "update", "http")]
public record GcloudSchedulerJobsUpdateHttpOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--attempt-deadline")]
    public string? AttemptDeadline { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--http-method")]
    public string? HttpMethod { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [BooleanCommandSwitch("--clear-auth-token")]
    public bool? ClearAuthToken { get; set; }

    [CommandSwitch("--oauth-service-account-email")]
    public string? OauthServiceAccountEmail { get; set; }

    [CommandSwitch("--oauth-token-scope")]
    public string? OauthTokenScope { get; set; }

    [CommandSwitch("--oidc-service-account-email")]
    public string? OidcServiceAccountEmail { get; set; }

    [CommandSwitch("--oidc-token-audience")]
    public string? OidcTokenAudience { get; set; }

    [BooleanCommandSwitch("--clear-headers")]
    public bool? ClearHeaders { get; set; }

    [CommandSwitch("--remove-headers")]
    public string[]? RemoveHeaders { get; set; }

    [CommandSwitch("--update-headers")]
    public IEnumerable<KeyValue>? UpdateHeaders { get; set; }

    [BooleanCommandSwitch("--clear-max-backoff")]
    public bool? ClearMaxBackoff { get; set; }

    [CommandSwitch("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [BooleanCommandSwitch("--clear-max-doublings")]
    public bool? ClearMaxDoublings { get; set; }

    [CommandSwitch("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [BooleanCommandSwitch("--clear-max-retry-attempts")]
    public bool? ClearMaxRetryAttempts { get; set; }

    [CommandSwitch("--max-retry-attempts")]
    public string? MaxRetryAttempts { get; set; }

    [BooleanCommandSwitch("--clear-max-retry-duration")]
    public bool? ClearMaxRetryDuration { get; set; }

    [CommandSwitch("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [BooleanCommandSwitch("--clear-message-body")]
    public bool? ClearMessageBody { get; set; }

    [CommandSwitch("--message-body")]
    public string? MessageBody { get; set; }

    [CommandSwitch("--message-body-from-file")]
    public string? MessageBodyFromFile { get; set; }

    [BooleanCommandSwitch("--clear-min-backoff")]
    public bool? ClearMinBackoff { get; set; }

    [CommandSwitch("--min-backoff")]
    public string? MinBackoff { get; set; }

    [BooleanCommandSwitch("--clear-time-zone")]
    public bool? ClearTimeZone { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }
}