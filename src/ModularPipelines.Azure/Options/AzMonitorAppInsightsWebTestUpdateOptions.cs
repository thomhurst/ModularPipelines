using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "web-test", "update")]
public record AzMonitorAppInsightsWebTestUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--content-validation")]
    public string? ContentValidation { get; set; }

    [CommandSwitch("--defined-web-test-name")]
    public string? DefinedWebTestName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--expected-status-code")]
    public string? ExpectedStatusCode { get; set; }

    [BooleanCommandSwitch("--follow-redirects")]
    public bool? FollowRedirects { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--frequency")]
    public string? Frequency { get; set; }

    [CommandSwitch("--headers")]
    public string? Headers { get; set; }

    [CommandSwitch("--http-verb")]
    public string? HttpVerb { get; set; }

    [BooleanCommandSwitch("--ignore-status-code")]
    public bool? IgnoreStatusCode { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--locations")]
    public string? Locations { get; set; }

    [BooleanCommandSwitch("--parse-requests")]
    public bool? ParseRequests { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--request-body")]
    public string? RequestBody { get; set; }

    [CommandSwitch("--request-url")]
    public string? RequestUrl { get; set; }

    [BooleanCommandSwitch("--retry-enabled")]
    public bool? RetryEnabled { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--ssl-check")]
    public bool? SslCheck { get; set; }

    [CommandSwitch("--ssl-lifetime-check")]
    public string? SslLifetimeCheck { get; set; }

    [CommandSwitch("--synthetic-monitor-id")]
    public string? SyntheticMonitorId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--web-test")]
    public string? WebTest { get; set; }

    [CommandSwitch("--web-test-kind")]
    public string? WebTestKind { get; set; }
}