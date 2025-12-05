using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "get-item")]
public record AwsDynamodbGetItemOptions(
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--key")] IEnumerable<KeyValue> Key
) : AwsOptions
{
    [CliOption("--attributes-to-get")]
    public string[]? AttributesToGet { get; set; }

    [CliOption("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CliOption("--projection-expression")]
    public string? ProjectionExpression { get; set; }

    [CliOption("--expression-attribute-names")]
    public IEnumerable<KeyValue>? ExpressionAttributeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}