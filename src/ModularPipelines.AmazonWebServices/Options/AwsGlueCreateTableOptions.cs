using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-table")]
public record AwsGlueCreateTableOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-input")] string TableInput
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--partition-indexes")]
    public string[]? PartitionIndexes { get; set; }

    [CliOption("--transaction-id")]
    public string? TransactionId { get; set; }

    [CliOption("--open-table-format-input")]
    public string? OpenTableFormatInput { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}