using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hybridaks", "upgrade")]
public record AzHybridaksUpgradeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--node-image-only")]
    public bool? NodeImageOnly { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}