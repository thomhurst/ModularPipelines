using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hybridaks", "upgrade")]
public record AzHybridaksUpgradeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--node-image-only")]
    public bool? NodeImageOnly { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}