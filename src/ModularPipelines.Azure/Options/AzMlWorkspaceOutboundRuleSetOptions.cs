using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "workspace", "outbound-rule", "set")]
public record AzMlWorkspaceOutboundRuleSetOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule")] string Rule,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--port-ranges")]
    public string? PortRanges { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--service-resource-id")]
    public string? ServiceResourceId { get; set; }

    [CommandSwitch("--service-tag")]
    public string? ServiceTag { get; set; }

    [BooleanCommandSwitch("--spark-enabled")]
    public bool? SparkEnabled { get; set; }

    [CommandSwitch("--subresource-target")]
    public string? SubresourceTarget { get; set; }
}