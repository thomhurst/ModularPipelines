using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "connection-profiles", "create", "cloudsql")]
public record GcloudDatabaseMigrationConnectionProfilesCreateCloudsqlOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--database-version")] string DatabaseVersion,
[property: CommandSwitch("--source-id")] string SourceId,
[property: CommandSwitch("--tier")] string Tier
) : GcloudOptions
{
    [CommandSwitch("--activation-policy")]
    public string? ActivationPolicy { get; set; }

    [CommandSwitch("--allocated-ip-range")]
    public string? AllocatedIpRange { get; set; }

    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--authorized-networks")]
    public string[]? AuthorizedNetworks { get; set; }

    [BooleanCommandSwitch("--auto-storage-increase")]
    public bool? AutoStorageIncrease { get; set; }

    [CommandSwitch("--availability-type")]
    public string? AvailabilityType { get; set; }

    [CommandSwitch("--data-disk-size")]
    public string? DataDiskSize { get; set; }

    [CommandSwitch("--data-disk-type")]
    public string? DataDiskType { get; set; }

    [CommandSwitch("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--edition")]
    public string? Edition { get; set; }

    [BooleanCommandSwitch("--enable-ip-v4")]
    public bool? EnableIpV4 { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--private-network")]
    public string? PrivateNetwork { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [BooleanCommandSwitch("--require-ssl")]
    public bool? RequireSsl { get; set; }

    [CommandSwitch("--root-password")]
    public string? RootPassword { get; set; }

    [CommandSwitch("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CommandSwitch("--storage-auto-resize-limit")]
    public string? StorageAutoResizeLimit { get; set; }

    [CommandSwitch("--user-labels")]
    public IEnumerable<KeyValue>? UserLabels { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--cmek-key")]
    public string? CmekKey { get; set; }

    [CommandSwitch("--cmek-keyring")]
    public string? CmekKeyring { get; set; }

    [CommandSwitch("--cmek-project")]
    public string? CmekProject { get; set; }
}