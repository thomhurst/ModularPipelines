using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyspaces", "update-table")]
public record AwsKeyspacesUpdateTableOptions(
[property: CommandSwitch("--keyspace-name")] string KeyspaceName,
[property: CommandSwitch("--table-name")] string TableName
) : AwsOptions
{
    [CommandSwitch("--add-columns")]
    public string[]? AddColumns { get; set; }

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

    [CommandSwitch("--client-side-timestamps")]
    public string? ClientSideTimestamps { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}