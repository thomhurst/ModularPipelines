using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance", "applyupdate", "create-or-update")]
public record AzMaintenanceApplyupdateCreateOrUpdateOptions : AzOptions
{
    [CommandSwitch("--apply-update-name")]
    public string? ApplyUpdateName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--last-update-time")]
    public string? LastUpdateTime { get; set; }

    [CommandSwitch("--provider-name")]
    public string? ProviderName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}