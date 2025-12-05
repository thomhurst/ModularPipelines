using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "create", "http")]
public record GcloudSchedulerJobsCreateHttpOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location,
[property: CliOption("--schedule")] string Schedule,
[property: CliOption("--uri")] string Uri
) : GcloudOptions
{
    [CliOption("--attempt-deadline")]
    public string? AttemptDeadline { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CliOption("--http-method")]
    public string? HttpMethod { get; set; }

    [CliOption("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [CliOption("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [CliOption("--max-retry-attempts")]
    public string? MaxRetryAttempts { get; set; }

    [CliOption("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [CliOption("--min-backoff")]
    public string? MinBackoff { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }

    [CliOption("--message-body")]
    public string? MessageBody { get; set; }

    [CliOption("--message-body-from-file")]
    public string? MessageBodyFromFile { get; set; }

    [CliOption("--oauth-service-account-email")]
    public string? OauthServiceAccountEmail { get; set; }

    [CliOption("--oauth-token-scope")]
    public string? OauthTokenScope { get; set; }

    [CliOption("--oidc-service-account-email")]
    public string? OidcServiceAccountEmail { get; set; }

    [CliOption("--oidc-token-audience")]
    public string? OidcTokenAudience { get; set; }
}