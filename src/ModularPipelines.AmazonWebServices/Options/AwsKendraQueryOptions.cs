using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "query")]
public record AwsKendraQueryOptions(
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--query-text")]
    public string? QueryText { get; set; }

    [CliOption("--attribute-filter")]
    public string? AttributeFilter { get; set; }

    [CliOption("--facets")]
    public string[]? Facets { get; set; }

    [CliOption("--requested-document-attributes")]
    public string[]? RequestedDocumentAttributes { get; set; }

    [CliOption("--query-result-type-filter")]
    public string? QueryResultTypeFilter { get; set; }

    [CliOption("--document-relevance-override-configurations")]
    public string[]? DocumentRelevanceOverrideConfigurations { get; set; }

    [CliOption("--page-number")]
    public int? PageNumber { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--sorting-configuration")]
    public string? SortingConfiguration { get; set; }

    [CliOption("--sorting-configurations")]
    public string[]? SortingConfigurations { get; set; }

    [CliOption("--user-context")]
    public string? UserContext { get; set; }

    [CliOption("--visitor-id")]
    public string? VisitorId { get; set; }

    [CliOption("--spell-correction-configuration")]
    public string? SpellCorrectionConfiguration { get; set; }

    [CliOption("--collapse-configuration")]
    public string? CollapseConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}