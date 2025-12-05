using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mesh", "service-replica", "show")]
public record AzMeshServiceReplicaShowOptions : AzOptions
{
    [CliOption("--app-name")]
    public string? AppName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-name")]
    public string? ServiceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}