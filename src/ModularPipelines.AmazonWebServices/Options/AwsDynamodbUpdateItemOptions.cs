using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "update-item")]
public record AwsDynamodbUpdateItemOptions(
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--key")] IEnumerable<KeyValue> Key
) : AwsOptions
{
    [CliOption("--attribute-updates")]
    public IEnumerable<KeyValue>? AttributeUpdates { get; set; }

    [CliOption("--expected")]
    public IEnumerable<KeyValue>? Expected { get; set; }

    [CliOption("--conditional-operator")]
    public string? ConditionalOperator { get; set; }

    [CliOption("--return-values")]
    public string? ReturnValues { get; set; }

    [CliOption("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CliOption("--return-item-collection-metrics")]
    public string? ReturnItemCollectionMetrics { get; set; }

    [CliOption("--update-expression")]
    public string? UpdateExpression { get; set; }

    [CliOption("--condition-expression")]
    public string? ConditionExpression { get; set; }

    [CliOption("--expression-attribute-names")]
    public IEnumerable<KeyValue>? ExpressionAttributeNames { get; set; }

    [CliOption("--expression-attribute-values")]
    public IEnumerable<KeyValue>? ExpressionAttributeValues { get; set; }

    [CliOption("--return-values-on-condition-check-failure")]
    public string? ReturnValuesOnConditionCheckFailure { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}