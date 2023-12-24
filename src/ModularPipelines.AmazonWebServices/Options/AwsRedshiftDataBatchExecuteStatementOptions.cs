using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-data", "batch-execute-statement")]
public record AwsRedshiftDataBatchExecuteStatementOptions(
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--sqls")] string[] Sqls
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CommandSwitch("--db-user")]
    public string? DbUser { get; set; }

    [CommandSwitch("--secret-arn")]
    public string? SecretArn { get; set; }

    [CommandSwitch("--statement-name")]
    public string? StatementName { get; set; }

    [CommandSwitch("--workgroup-name")]
    public string? WorkgroupName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}