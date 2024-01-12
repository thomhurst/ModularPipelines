using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "create-http-task")]
public record GcloudTasksCreateHttpTaskOptions(
[property: PositionalArgument] string TaskId,
[property: CommandSwitch("--queue")] string Queue,
[property: CommandSwitch("--url")] string Url
) : GcloudOptions
{
    [CommandSwitch("--header")]
    public string? Header { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--method")]
    public string? Method { get; set; }

    [CommandSwitch("--schedule-time")]
    public string? ScheduleTime { get; set; }

    [CommandSwitch("--body-content")]
    public string? BodyContent { get; set; }

    [CommandSwitch("--body-file")]
    public string? BodyFile { get; set; }

    [CommandSwitch("--oauth-service-account-email")]
    public string? OauthServiceAccountEmail { get; set; }

    [CommandSwitch("--oauth-token-scope")]
    public string? OauthTokenScope { get; set; }

    [CommandSwitch("--oidc-service-account-email")]
    public string? OidcServiceAccountEmail { get; set; }

    [CommandSwitch("--oidc-token-audience")]
    public string? OidcTokenAudience { get; set; }
}