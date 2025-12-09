using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "sql", "pool", "classification", "update")]
public record AzSynapseSqlPoolClassificationUpdateOptions(
[property: CliOption("--column")] string Column,
[property: CliOption("--schema")] string Schema,
[property: CliOption("--table")] string Table
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--information-type")]
    public string? InformationType { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}