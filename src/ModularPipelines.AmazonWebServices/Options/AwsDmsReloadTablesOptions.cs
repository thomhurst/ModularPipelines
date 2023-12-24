using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "reload-tables")]
public record AwsDmsReloadTablesOptions(
[property: CommandSwitch("--replication-task-arn")] string ReplicationTaskArn,
[property: CommandSwitch("--tables-to-reload")] string[] TablesToReload
) : AwsOptions
{
    [CommandSwitch("--reload-option")]
    public string? ReloadOption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}