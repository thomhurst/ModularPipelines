using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds-data", "execute-statement")]
public record AwsRdsDataExecuteStatementOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--secret-arn")] string SecretArn,
[property: CommandSwitch("--sql")] string Sql
) : AwsOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--schema")]
    public string? Schema { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--transaction-id")]
    public string? TransactionId { get; set; }

    [CommandSwitch("--result-set-options")]
    public string? ResultSetOptions { get; set; }

    [CommandSwitch("--format-records-as")]
    public string? FormatRecordsAs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}