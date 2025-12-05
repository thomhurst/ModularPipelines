using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("advisor", "recommendation", "list")]
public record AzAdvisorRecommendationListOptions : AzOptions
{
    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--refresh")]
    public string? Refresh { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}