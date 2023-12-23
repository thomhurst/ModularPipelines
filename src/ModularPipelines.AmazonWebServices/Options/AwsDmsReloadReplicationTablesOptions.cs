using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "reload-replication-tables")]
public record AwsDmsReloadReplicationTablesOptions(
[property: CommandSwitch("--replication-config-arn")] string ReplicationConfigArn,
[property: CommandSwitch("--tables-to-reload")] string[] TablesToReload
) : AwsOptions
{
    [CommandSwitch("--reload-option")]
    public string? ReloadOption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}