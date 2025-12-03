using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-data", "batch-execute-statement")]
public record AwsRedshiftDataBatchExecuteStatementOptions(
[property: CliOption("--database")] string Database,
[property: CliOption("--sqls")] string[] Sqls
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--db-user")]
    public string? DbUser { get; set; }

    [CliOption("--secret-arn")]
    public string? SecretArn { get; set; }

    [CliOption("--statement-name")]
    public string? StatementName { get; set; }

    [CliOption("--workgroup-name")]
    public string? WorkgroupName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}