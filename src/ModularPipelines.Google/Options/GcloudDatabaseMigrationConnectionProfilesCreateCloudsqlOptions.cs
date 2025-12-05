using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles", "create", "cloudsql")]
public record GcloudDatabaseMigrationConnectionProfilesCreateCloudsqlOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Region,
[property: CliOption("--database-version")] string DatabaseVersion,
[property: CliOption("--source-id")] string SourceId,
[property: CliOption("--tier")] string Tier
) : GcloudOptions
{
    [CliOption("--activation-policy")]
    public string? ActivationPolicy { get; set; }

    [CliOption("--allocated-ip-range")]
    public string? AllocatedIpRange { get; set; }

    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--authorized-networks")]
    public string[]? AuthorizedNetworks { get; set; }

    [CliFlag("--auto-storage-increase")]
    public bool? AutoStorageIncrease { get; set; }

    [CliOption("--availability-type")]
    public string? AvailabilityType { get; set; }

    [CliOption("--data-disk-size")]
    public string? DataDiskSize { get; set; }

    [CliOption("--data-disk-type")]
    public string? DataDiskType { get; set; }

    [CliOption("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliFlag("--enable-ip-v4")]
    public bool? EnableIpV4 { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--private-network")]
    public string? PrivateNetwork { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliFlag("--require-ssl")]
    public bool? RequireSsl { get; set; }

    [CliOption("--root-password")]
    public string? RootPassword { get; set; }

    [CliOption("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CliOption("--storage-auto-resize-limit")]
    public string? StorageAutoResizeLimit { get; set; }

    [CliOption("--user-labels")]
    public IEnumerable<KeyValue>? UserLabels { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--cmek-key")]
    public string? CmekKey { get; set; }

    [CliOption("--cmek-keyring")]
    public string? CmekKeyring { get; set; }

    [CliOption("--cmek-project")]
    public string? CmekProject { get; set; }
}