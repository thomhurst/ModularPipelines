using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "group", "create")]
public record AzNetworkManagerGroupCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--network-manager-name")] string NetworkManagerName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }
}