using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "integration-runtime-node", "update")]
public record AzSynapseIntegrationRuntimeNodeUpdateOptions(
[property: CliOption("--auto-update")] string AutoUpdate,
[property: CliOption("--node-name")] string NodeName,
[property: CliOption("--update-delay-offset")] string UpdateDelayOffset
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}