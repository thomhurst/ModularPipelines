using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "app-insights", "web-test", "update")]
public record AzMonitorAppInsightsWebTestUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--content-validation")]
    public string? ContentValidation { get; set; }

    [CliOption("--defined-web-test-name")]
    public string? DefinedWebTestName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--expected-status-code")]
    public string? ExpectedStatusCode { get; set; }

    [CliFlag("--follow-redirects")]
    public bool? FollowRedirects { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--frequency")]
    public string? Frequency { get; set; }

    [CliOption("--headers")]
    public string? Headers { get; set; }

    [CliOption("--http-verb")]
    public string? HttpVerb { get; set; }

    [CliFlag("--ignore-status-code")]
    public bool? IgnoreStatusCode { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--locations")]
    public string? Locations { get; set; }

    [CliFlag("--parse-requests")]
    public bool? ParseRequests { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--request-body")]
    public string? RequestBody { get; set; }

    [CliOption("--request-url")]
    public string? RequestUrl { get; set; }

    [CliFlag("--retry-enabled")]
    public bool? RetryEnabled { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--ssl-check")]
    public bool? SslCheck { get; set; }

    [CliOption("--ssl-lifetime-check")]
    public string? SslLifetimeCheck { get; set; }

    [CliOption("--synthetic-monitor-id")]
    public string? SyntheticMonitorId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--web-test")]
    public string? WebTest { get; set; }

    [CliOption("--web-test-kind")]
    public string? WebTestKind { get; set; }
}