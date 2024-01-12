using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "update")]
public record GcloudMetastoreServicesUpdateOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--data-catalog-sync")]
    public bool? DataCatalogSync { get; set; }

    [CommandSwitch("--endpoint-protocol")]
    public string? EndpointProtocol { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--instance-size")]
    public string? InstanceSize { get; set; }

    [BooleanCommandSwitch("extra-large")]
    public bool? ExtraLarge { get; set; }

    [BooleanCommandSwitch("extra-small")]
    public bool? ExtraSmall { get; set; }

    [BooleanCommandSwitch("large")]
    public bool? Large { get; set; }

    [BooleanCommandSwitch("medium")]
    public bool? Medium { get; set; }

    [BooleanCommandSwitch("small")]
    public bool? Small { get; set; }

    [CommandSwitch("--scaling-factor")]
    public string? ScalingFactor { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [BooleanCommandSwitch("developer")]
    public bool? Developer { get; set; }

    [BooleanCommandSwitch("enterprise")]
    public bool? Enterprise { get; set; }

    [CommandSwitch("--kerberos-principal")]
    public string? KerberosPrincipal { get; set; }

    [CommandSwitch("--keytab")]
    public string? Keytab { get; set; }

    [CommandSwitch("--krb5-config")]
    public string? Krb5Config { get; set; }

    [CommandSwitch("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CommandSwitch("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }

    [CommandSwitch("--update-auxiliary-versions-from-file")]
    public string? UpdateAuxiliaryVersionsFromFile { get; set; }

    [CommandSwitch("--add-auxiliary-versions")]
    public string[]? AddAuxiliaryVersions { get; set; }

    [BooleanCommandSwitch("--clear-auxiliary-versions")]
    public bool? ClearAuxiliaryVersions { get; set; }

    [CommandSwitch("--update-hive-metastore-configs-from-file")]
    public string? UpdateHiveMetastoreConfigsFromFile { get; set; }

    [CommandSwitch("--update-hive-metastore-configs")]
    public IEnumerable<KeyValue>? UpdateHiveMetastoreConfigs { get; set; }

    [BooleanCommandSwitch("--clear-hive-metastore-configs")]
    public bool? ClearHiveMetastoreConfigs { get; set; }

    [CommandSwitch("--remove-hive-metastore-configs")]
    public string[]? RemoveHiveMetastoreConfigs { get; set; }
}