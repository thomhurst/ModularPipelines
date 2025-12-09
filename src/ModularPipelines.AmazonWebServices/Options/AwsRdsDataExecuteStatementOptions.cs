using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds-data", "execute-statement")]
public record AwsRdsDataExecuteStatementOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--secret-arn")] string SecretArn,
[property: CliOption("--sql")] string Sql
) : AwsOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--transaction-id")]
    public string? TransactionId { get; set; }

    [CliOption("--result-set-options")]
    public string? ResultSetOptions { get; set; }

    [CliOption("--format-records-as")]
    public string? FormatRecordsAs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}