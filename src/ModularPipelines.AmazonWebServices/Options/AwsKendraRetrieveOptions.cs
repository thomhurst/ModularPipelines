using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "retrieve")]
public record AwsKendraRetrieveOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--query-text")] string QueryText
) : AwsOptions
{
    [CliOption("--attribute-filter")]
    public string? AttributeFilter { get; set; }

    [CliOption("--requested-document-attributes")]
    public string[]? RequestedDocumentAttributes { get; set; }

    [CliOption("--document-relevance-override-configurations")]
    public string[]? DocumentRelevanceOverrideConfigurations { get; set; }

    [CliOption("--page-number")]
    public int? PageNumber { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--user-context")]
    public string? UserContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}