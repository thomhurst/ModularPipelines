using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "batch-get-item")]
public record AwsDynamodbBatchGetItemOptions(
[property: CliOption("--request-items")] IEnumerable<KeyValue> RequestItems
) : AwsOptions
{
    [CliOption("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}