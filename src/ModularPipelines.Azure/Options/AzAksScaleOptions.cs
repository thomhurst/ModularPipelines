using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "scale")]
public record AzAksScaleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--node-count")] int NodeCount,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nodepool-name")]
    public string? NodepoolName { get; set; }
}