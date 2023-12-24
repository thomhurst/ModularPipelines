using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "stop-db-instance-automated-backups-replication")]
public record AwsRdsStopDbInstanceAutomatedBackupsReplicationOptions(
[property: CommandSwitch("--source-db-instance-arn")] string SourceDbInstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}