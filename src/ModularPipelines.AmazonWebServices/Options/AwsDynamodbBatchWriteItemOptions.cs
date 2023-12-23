using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "batch-write-item")]
public record AwsDynamodbBatchWriteItemOptions(
[property: CommandSwitch("--request-items")] IEnumerable<KeyValue> RequestItems
) : AwsOptions
{
    [CommandSwitch("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CommandSwitch("--return-item-collection-metrics")]
    public string? ReturnItemCollectionMetrics { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}