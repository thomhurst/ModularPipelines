using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "job", "create")]
public record AzIotHubJobCreateOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--job-type")] string JobType
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--mct")]
    public string? Mct { get; set; }

    [CliOption("--method-name")]
    public string? MethodName { get; set; }

    [CliOption("--method-payload")]
    public string? MethodPayload { get; set; }

    [CliOption("--method-response-timeout")]
    public string? MethodResponseTimeout { get; set; }

    [CliOption("--patch")]
    public string? Patch { get; set; }

    [CliOption("--query-condition")]
    public string? QueryCondition { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start")]
    public string? Start { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }

    [CliFlag("--wait")]
    public bool? Wait { get; set; }
}