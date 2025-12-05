using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "batch-delete-partition")]
public record AwsGlueBatchDeletePartitionOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--partitions-to-delete")] string[] PartitionsToDelete
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}