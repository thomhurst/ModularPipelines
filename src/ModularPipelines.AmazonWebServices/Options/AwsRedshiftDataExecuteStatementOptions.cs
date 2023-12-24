using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-data", "execute-statement")]
public record AwsRedshiftDataExecuteStatementOptions(
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--sql")] string Sql
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CommandSwitch("--db-user")]
    public string? DbUser { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--secret-arn")]
    public string? SecretArn { get; set; }

    [CommandSwitch("--statement-name")]
    public string? StatementName { get; set; }

    [CommandSwitch("--workgroup-name")]
    public string? WorkgroupName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}