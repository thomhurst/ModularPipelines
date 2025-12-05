using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "batch-execute-statement")]
public record AwsDynamodbBatchExecuteStatementOptions(
[property: CliOption("--statements")] string[] Statements
) : AwsOptions
{
    [CliOption("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}