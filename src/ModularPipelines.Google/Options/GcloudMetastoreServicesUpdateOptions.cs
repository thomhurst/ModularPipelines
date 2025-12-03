using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "update")]
public record GcloudMetastoreServicesUpdateOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--data-catalog-sync")]
    public bool? DataCatalogSync { get; set; }

    [CliOption("--endpoint-protocol")]
    public string? EndpointProtocol { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--instance-size")]
    public string? InstanceSize { get; set; }

    [CliFlag("extra-large")]
    public bool? ExtraLarge { get; set; }

    [CliFlag("extra-small")]
    public bool? ExtraSmall { get; set; }

    [CliFlag("large")]
    public bool? Large { get; set; }

    [CliFlag("medium")]
    public bool? Medium { get; set; }

    [CliFlag("small")]
    public bool? Small { get; set; }

    [CliOption("--scaling-factor")]
    public string? ScalingFactor { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliFlag("developer")]
    public bool? Developer { get; set; }

    [CliFlag("enterprise")]
    public bool? Enterprise { get; set; }

    [CliOption("--kerberos-principal")]
    public string? KerberosPrincipal { get; set; }

    [CliOption("--keytab")]
    public string? Keytab { get; set; }

    [CliOption("--krb5-config")]
    public string? Krb5Config { get; set; }

    [CliOption("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CliOption("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }

    [CliOption("--update-auxiliary-versions-from-file")]
    public string? UpdateAuxiliaryVersionsFromFile { get; set; }

    [CliOption("--add-auxiliary-versions")]
    public string[]? AddAuxiliaryVersions { get; set; }

    [CliFlag("--clear-auxiliary-versions")]
    public bool? ClearAuxiliaryVersions { get; set; }

    [CliOption("--update-hive-metastore-configs-from-file")]
    public string? UpdateHiveMetastoreConfigsFromFile { get; set; }

    [CliOption("--update-hive-metastore-configs")]
    public IEnumerable<KeyValue>? UpdateHiveMetastoreConfigs { get; set; }

    [CliFlag("--clear-hive-metastore-configs")]
    public bool? ClearHiveMetastoreConfigs { get; set; }

    [CliOption("--remove-hive-metastore-configs")]
    public string[]? RemoveHiveMetastoreConfigs { get; set; }
}