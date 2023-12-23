using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "delete-item")]
public record AwsDynamodbDeleteItemOptions(
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--key")] IEnumerable<KeyValue> Key
) : AwsOptions
{
    [CommandSwitch("--expected")]
    public IEnumerable<KeyValue>? Expected { get; set; }

    [CommandSwitch("--conditional-operator")]
    public string? ConditionalOperator { get; set; }

    [CommandSwitch("--return-values")]
    public string? ReturnValues { get; set; }

    [CommandSwitch("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CommandSwitch("--return-item-collection-metrics")]
    public string? ReturnItemCollectionMetrics { get; set; }

    [CommandSwitch("--condition-expression")]
    public string? ConditionExpression { get; set; }

    [CommandSwitch("--expression-attribute-names")]
    public IEnumerable<KeyValue>? ExpressionAttributeNames { get; set; }

    [CommandSwitch("--expression-attribute-values")]
    public IEnumerable<KeyValue>? ExpressionAttributeValues { get; set; }

    [CommandSwitch("--return-values-on-condition-check-failure")]
    public string? ReturnValuesOnConditionCheckFailure { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}