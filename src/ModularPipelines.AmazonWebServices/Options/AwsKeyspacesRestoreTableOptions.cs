using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyspaces", "restore-table")]
public record AwsKeyspacesRestoreTableOptions(
[property: CommandSwitch("--source-keyspace-name")] string SourceKeyspaceName,
[property: CommandSwitch("--source-table-name")] string SourceTableName,
[property: CommandSwitch("--target-keyspace-name")] string TargetKeyspaceName,
[property: CommandSwitch("--target-table-name")] string TargetTableName
) : AwsOptions
{
    [CommandSwitch("--restore-timestamp")]
    public long? RestoreTimestamp { get; set; }

    [CommandSwitch("--capacity-specification-override")]
    public string? CapacitySpecificationOverride { get; set; }

    [CommandSwitch("--encryption-specification-override")]
    public string? EncryptionSpecificationOverride { get; set; }

    [CommandSwitch("--point-in-time-recovery-override")]
    public string? PointInTimeRecoveryOverride { get; set; }

    [CommandSwitch("--tags-override")]
    public string[]? TagsOverride { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}