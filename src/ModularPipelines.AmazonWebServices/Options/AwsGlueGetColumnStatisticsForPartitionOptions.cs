using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-column-statistics-for-partition")]
public record AwsGlueGetColumnStatisticsForPartitionOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--partition-values")] string[] PartitionValues,
[property: CliOption("--column-names")] string[] ColumnNames
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}