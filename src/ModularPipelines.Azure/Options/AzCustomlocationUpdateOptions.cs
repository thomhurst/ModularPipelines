using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("customlocation", "update")]
public record AzCustomlocationUpdateOptions(
[property: CliOption("--cluster-extension-ids")] string ClusterExtensionIds,
[property: CliOption("--host-resource-id")] string HostResourceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}