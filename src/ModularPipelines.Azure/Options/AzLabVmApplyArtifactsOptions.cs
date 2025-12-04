using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("lab", "vm", "apply-artifacts")]
public record AzLabVmApplyArtifactsOptions(
[property: CliOption("--lab-name")] string LabName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--artifacts")]
    public string? Artifacts { get; set; }
}