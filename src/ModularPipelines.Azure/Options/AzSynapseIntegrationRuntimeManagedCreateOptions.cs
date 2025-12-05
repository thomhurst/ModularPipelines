using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "integration-runtime", "managed", "create")]
public record AzSynapseIntegrationRuntimeManagedCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--compute-type")]
    public string? ComputeType { get; set; }

    [CliOption("--core-count")]
    public int? CoreCount { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--time-to-live")]
    public string? TimeToLive { get; set; }
}