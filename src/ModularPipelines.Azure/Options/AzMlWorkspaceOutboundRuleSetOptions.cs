using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "workspace", "outbound-rule", "set")]
public record AzMlWorkspaceOutboundRuleSetOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule")] string Rule,
[property: CliOption("--type")] string Type,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--port-ranges")]
    public string? PortRanges { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--service-resource-id")]
    public string? ServiceResourceId { get; set; }

    [CliOption("--service-tag")]
    public string? ServiceTag { get; set; }

    [CliFlag("--spark-enabled")]
    public bool? SparkEnabled { get; set; }

    [CliOption("--subresource-target")]
    public string? SubresourceTarget { get; set; }
}