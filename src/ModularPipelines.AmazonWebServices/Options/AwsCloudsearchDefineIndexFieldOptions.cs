using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "define-index-field")]
public record AwsCloudsearchDefineIndexFieldOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--default-value")]
    public string? DefaultValue { get; set; }

    [CliFlag("--facet-enabled")]
    public bool? FacetEnabled { get; set; }

    [CliFlag("--search-enabled")]
    public bool? SearchEnabled { get; set; }

    [CliFlag("--return-enabled")]
    public bool? ReturnEnabled { get; set; }

    [CliFlag("--sort-enabled")]
    public bool? SortEnabled { get; set; }

    [CliOption("--source-field")]
    public string? SourceField { get; set; }

    [CliFlag("--highlight-enabled")]
    public bool? HighlightEnabled { get; set; }

    [CliOption("--analysis-scheme")]
    public string? AnalysisScheme { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}