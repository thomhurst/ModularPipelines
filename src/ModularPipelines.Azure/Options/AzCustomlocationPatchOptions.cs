using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customlocation", "patch")]
public record AzCustomlocationPatchOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--cluster-extension-ids")]
    public string? ClusterExtensionIds { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--host-resource-id")]
    public string? HostResourceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}