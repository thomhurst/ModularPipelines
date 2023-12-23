using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "get-item")]
public record AwsDynamodbGetItemOptions(
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--key")] IEnumerable<KeyValue> Key
) : AwsOptions
{
    [CommandSwitch("--attributes-to-get")]
    public string[]? AttributesToGet { get; set; }

    [CommandSwitch("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CommandSwitch("--projection-expression")]
    public string? ProjectionExpression { get; set; }

    [CommandSwitch("--expression-attribute-names")]
    public IEnumerable<KeyValue>? ExpressionAttributeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}