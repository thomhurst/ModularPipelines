using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource", "wait")]
public record AzResourceWaitOptions : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [BooleanCommandSwitch("--created")]
    public bool? Created { get; set; }

    [CommandSwitch("--custom")]
    public string? Custom { get; set; }

    [BooleanCommandSwitch("--deleted")]
    public bool? Deleted { get; set; }

    [BooleanCommandSwitch("--exists")]
    public bool? Exists { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--include-response-body")]
    public bool? IncludeResponseBody { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--parent")]
    public string? Parent { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--updated")]
    public bool? Updated { get; set; }
}