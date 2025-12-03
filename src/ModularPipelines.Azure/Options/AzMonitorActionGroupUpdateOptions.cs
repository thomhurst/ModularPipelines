using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "action-group", "update")]
public record AzMonitorActionGroupUpdateOptions : AzOptions
{
    [CliOption("--action-group-name")]
    public string? ActionGroupName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--add-action")]
    public string? AddAction { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--group-short-name")]
    public string? GroupShortName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--remove-action")]
    public string? RemoveAction { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}