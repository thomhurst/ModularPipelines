using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "execute-statement")]
public record AwsDynamodbExecuteStatementOptions(
[property: CommandSwitch("--statement")] string Statement
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--return-values-on-condition-check-failure")]
    public string? ReturnValuesOnConditionCheckFailure { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}