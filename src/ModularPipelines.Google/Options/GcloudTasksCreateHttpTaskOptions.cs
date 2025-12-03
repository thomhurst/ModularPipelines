using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "create-http-task")]
public record GcloudTasksCreateHttpTaskOptions(
[property: CliArgument] string TaskId,
[property: CliOption("--queue")] string Queue,
[property: CliOption("--url")] string Url
) : GcloudOptions
{
    [CliOption("--header")]
    public string? Header { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--method")]
    public string? Method { get; set; }

    [CliOption("--schedule-time")]
    public string? ScheduleTime { get; set; }

    [CliOption("--body-content")]
    public string? BodyContent { get; set; }

    [CliOption("--body-file")]
    public string? BodyFile { get; set; }

    [CliOption("--oauth-service-account-email")]
    public string? OauthServiceAccountEmail { get; set; }

    [CliOption("--oauth-token-scope")]
    public string? OauthTokenScope { get; set; }

    [CliOption("--oidc-service-account-email")]
    public string? OidcServiceAccountEmail { get; set; }

    [CliOption("--oidc-token-audience")]
    public string? OidcTokenAudience { get; set; }
}