using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds-data", "batch-execute-statement")]
public record AwsRdsDataBatchExecuteStatementOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--secret-arn")] string SecretArn,
[property: CliOption("--sql")] string Sql
) : AwsOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--parameter-sets")]
    public string[]? ParameterSets { get; set; }

    [CliOption("--transaction-id")]
    public string? TransactionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}