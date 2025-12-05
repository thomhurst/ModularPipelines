using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "list-transactions")]
public record AwsLakeformationListTransactionsOptions : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--status-filter")]
    public string? StatusFilter { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}