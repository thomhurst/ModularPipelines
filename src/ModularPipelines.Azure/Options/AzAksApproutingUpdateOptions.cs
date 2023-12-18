using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "approuting", "update")]
public record AzAksApproutingUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--attach-kv")]
    public bool? AttachKv { get; set; }

    [BooleanCommandSwitch("--enable-kv")]
    public bool? EnableKv { get; set; }
}