using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logic", "integration-account", "map", "update")]
public record AzLogicIntegrationAccountMapUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--integration-account")]
    public int? IntegrationAccount { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--map-content")]
    public string? MapContent { get; set; }

    [CliOption("--map-name")]
    public string? MapName { get; set; }

    [CliOption("--map-type")]
    public string? MapType { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}