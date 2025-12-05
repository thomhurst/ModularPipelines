using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maintenance", "applyupdate", "update")]
public record AzMaintenanceApplyupdateUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--apply-update-name")]
    public string? ApplyUpdateName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--last-update-time")]
    public string? LastUpdateTime { get; set; }

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}