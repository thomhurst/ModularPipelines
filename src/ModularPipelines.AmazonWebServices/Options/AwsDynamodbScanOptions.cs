using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "scan")]
public record AwsDynamodbScanOptions(
[property: CommandSwitch("--table-name")] string TableName
) : AwsOptions
{
    [CommandSwitch("--index-name")]
    public string? IndexName { get; set; }

    [CommandSwitch("--attributes-to-get")]
    public string[]? AttributesToGet { get; set; }

    [CommandSwitch("--select")]
    public string? Select { get; set; }

    [CommandSwitch("--scan-filter")]
    public IEnumerable<KeyValue>? ScanFilter { get; set; }

    [CommandSwitch("--conditional-operator")]
    public string? ConditionalOperator { get; set; }

    [CommandSwitch("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CommandSwitch("--total-segments")]
    public int? TotalSegments { get; set; }

    [CommandSwitch("--segment")]
    public int? Segment { get; set; }

    [CommandSwitch("--projection-expression")]
    public string? ProjectionExpression { get; set; }

    [CommandSwitch("--filter-expression")]
    public string? FilterExpression { get; set; }

    [CommandSwitch("--expression-attribute-names")]
    public IEnumerable<KeyValue>? ExpressionAttributeNames { get; set; }

    [CommandSwitch("--expression-attribute-values")]
    public IEnumerable<KeyValue>? ExpressionAttributeValues { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}