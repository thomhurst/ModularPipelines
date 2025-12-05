using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "stop-db-instance-automated-backups-replication")]
public record AwsRdsStopDbInstanceAutomatedBackupsReplicationOptions(
[property: CliOption("--source-db-instance-arn")] string SourceDbInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}