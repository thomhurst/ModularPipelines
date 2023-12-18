using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance", "applyupdate", "create")]
public record AzMaintenanceApplyupdateCreateOptions(
[property: CommandSwitch("--provider-name")] string ProviderName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions
{
    [CommandSwitch("--apply-update-name")]
    public string? ApplyUpdateName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--last-update-time")]
    public string? LastUpdateTime { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

