using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyspaces", "create-table")]
public record AwsKeyspacesCreateTableOptions(
[property: CommandSwitch("--keyspace-name")] string KeyspaceName,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--schema-definition")] string SchemaDefinition
) : AwsOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--capacity-specification")]
    public string? CapacitySpecification { get; set; }

    [CommandSwitch("--encryption-specification")]
    public string? EncryptionSpecification { get; set; }

    [CommandSwitch("--point-in-time-recovery")]
    public string? PointInTimeRecovery { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }

    [CommandSwitch("--default-time-to-live")]
    public int? DefaultTimeToLive { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-side-timestamps")]
    public string? ClientSideTimestamps { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}