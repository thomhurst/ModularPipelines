using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "computetarget", "attach", "kubernetes")]
public record AzMlComputetargetAttachKubernetesOptions(
[property: CliOption("--compute-resource-id")] string ComputeResourceId,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}