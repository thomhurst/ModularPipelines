using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "connection-profiles", "create", "alloydb")]
public record GcloudDatabaseMigrationConnectionProfilesCreateAlloydbOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--cpu-count")] string CpuCount,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--primary-id")] string PrimaryId
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--cluster-labels")]
    public IEnumerable<KeyValue>? ClusterLabels { get; set; }

    [CommandSwitch("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CommandSwitch("--database-version")]
    public string? DatabaseVersion { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--primary-labels")]
    public IEnumerable<KeyValue>? PrimaryLabels { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}