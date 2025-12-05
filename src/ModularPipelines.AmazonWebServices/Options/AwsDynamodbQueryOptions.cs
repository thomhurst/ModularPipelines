using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "query")]
public record AwsDynamodbQueryOptions(
[property: CliOption("--table-name")] string TableName
) : AwsOptions
{
    [CliOption("--index-name")]
    public string? IndexName { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--attributes-to-get")]
    public string[]? AttributesToGet { get; set; }

    [CliOption("--key-conditions")]
    public IEnumerable<KeyValue>? KeyConditions { get; set; }

    [CliOption("--query-filter")]
    public IEnumerable<KeyValue>? QueryFilter { get; set; }

    [CliOption("--conditional-operator")]
    public string? ConditionalOperator { get; set; }

    [CliOption("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CliOption("--projection-expression")]
    public string? ProjectionExpression { get; set; }

    [CliOption("--filter-expression")]
    public string? FilterExpression { get; set; }

    [CliOption("--key-condition-expression")]
    public string? KeyConditionExpression { get; set; }

    [CliOption("--expression-attribute-names")]
    public IEnumerable<KeyValue>? ExpressionAttributeNames { get; set; }

    [CliOption("--expression-attribute-values")]
    public IEnumerable<KeyValue>? ExpressionAttributeValues { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}