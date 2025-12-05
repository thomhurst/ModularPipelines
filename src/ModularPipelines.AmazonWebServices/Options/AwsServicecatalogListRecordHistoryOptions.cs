using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "list-record-history")]
public record AwsServicecatalogListRecordHistoryOptions : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--access-level-filter")]
    public string? AccessLevelFilter { get; set; }

    [CliOption("--search-filter")]
    public string? SearchFilter { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}