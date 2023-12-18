using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "job", "create")]
public record AzIotHubJobCreateOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--job-type")] string JobType
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--mct")]
    public string? Mct { get; set; }

    [CommandSwitch("--method-name")]
    public string? MethodName { get; set; }

    [CommandSwitch("--method-payload")]
    public string? MethodPayload { get; set; }

    [CommandSwitch("--method-response-timeout")]
    public string? MethodResponseTimeout { get; set; }

    [CommandSwitch("--patch")]
    public string? Patch { get; set; }

    [CommandSwitch("--query-condition")]
    public string? QueryCondition { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--start")]
    public string? Start { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }
}