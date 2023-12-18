using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance", "update", "list")]
public record AzMaintenanceUpdateListOptions(
[property: CommandSwitch("--provider-name")] string ProviderName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-parent-name")]
    public string? ResourceParentName { get; set; }

    [CommandSwitch("--resource-parent-type")]
    public string? ResourceParentType { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}