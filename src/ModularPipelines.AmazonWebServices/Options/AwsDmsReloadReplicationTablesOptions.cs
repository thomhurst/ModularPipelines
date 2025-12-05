using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "reload-replication-tables")]
public record AwsDmsReloadReplicationTablesOptions(
[property: CliOption("--replication-config-arn")] string ReplicationConfigArn,
[property: CliOption("--tables-to-reload")] string[] TablesToReload
) : AwsOptions
{
    [CliOption("--reload-option")]
    public string? ReloadOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}