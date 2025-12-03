using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "sql", "pool", "classification", "recommendation", "list")]
public record AzSynapseSqlPoolClassificationRecommendationListOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliFlag("--included-disabled")]
    public bool? IncludedDisabled { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}