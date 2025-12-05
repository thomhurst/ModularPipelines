using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-tables")]
public record AwsGlueGetTablesOptions(
[property: CliOption("--database-name")] string DatabaseName
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--expression")]
    public string? Expression { get; set; }

    [CliOption("--transaction-id")]
    public string? TransactionId { get; set; }

    [CliOption("--query-as-of-time")]
    public long? QueryAsOfTime { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}