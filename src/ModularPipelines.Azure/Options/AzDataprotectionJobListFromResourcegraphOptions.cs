using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "job", "list-from-resourcegraph")]
public record AzDataprotectionJobListFromResourcegraphOptions(
[property: CliOption("--datasource-type")] string DatasourceType
) : AzOptions
{
    [CliOption("--datasource-id")]
    public string? DatasourceId { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--operation")]
    public string? Operation { get; set; }

    [CliOption("--resource-groups")]
    public string? ResourceGroups { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--subscriptions")]
    public string? Subscriptions { get; set; }

    [CliOption("--vaults")]
    public string? Vaults { get; set; }
}