using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "reload-tables")]
public record AwsDmsReloadTablesOptions(
[property: CliOption("--replication-task-arn")] string ReplicationTaskArn,
[property: CliOption("--tables-to-reload")] string[] TablesToReload
) : AwsOptions
{
    [CliOption("--reload-option")]
    public string? ReloadOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}