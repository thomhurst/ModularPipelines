using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds-data", "batch-execute-statement")]
public record AwsRdsDataBatchExecuteStatementOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--secret-arn")] string SecretArn,
[property: CommandSwitch("--sql")] string Sql
) : AwsOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--schema")]
    public string? Schema { get; set; }

    [CommandSwitch("--parameter-sets")]
    public string[]? ParameterSets { get; set; }

    [CommandSwitch("--transaction-id")]
    public string? TransactionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}