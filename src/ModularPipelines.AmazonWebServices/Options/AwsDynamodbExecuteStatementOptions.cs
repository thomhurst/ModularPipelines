using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "execute-statement")]
public record AwsDynamodbExecuteStatementOptions(
[property: CliOption("--statement")] string Statement
) : AwsOptions
{
    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--return-values-on-condition-check-failure")]
    public string? ReturnValuesOnConditionCheckFailure { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}