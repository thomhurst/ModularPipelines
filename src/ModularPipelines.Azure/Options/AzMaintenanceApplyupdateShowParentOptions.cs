using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maintenance", "applyupdate", "show-parent")]
public record AzMaintenanceApplyupdateShowParentOptions : AzOptions
{
    [CliOption("--apply-update-name")]
    public string? ApplyUpdateName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--resource-parent-name")]
    public string? ResourceParentName { get; set; }

    [CliOption("--resource-parent-type")]
    public string? ResourceParentType { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}