using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "batch-write-item")]
public record AwsDynamodbBatchWriteItemOptions(
[property: CliOption("--request-items")] IEnumerable<KeyValue> RequestItems
) : AwsOptions
{
    [CliOption("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CliOption("--return-item-collection-metrics")]
    public string? ReturnItemCollectionMetrics { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}