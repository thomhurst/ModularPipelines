using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "create")]
public record GcloudComposerEnvironmentsCreateOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--airflow-configs")]
    public IEnumerable<KeyValue>? AirflowConfigs { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cloud-sql-machine-type")]
    public string? CloudSqlMachineType { get; set; }

    [CommandSwitch("--cloud-sql-preferred-zone")]
    public string? CloudSqlPreferredZone { get; set; }

    [BooleanCommandSwitch("--disable-logs-in-cloud-logging-only")]
    public bool? DisableLogsInCloudLoggingOnly { get; set; }

    [CommandSwitch("--disk-size")]
    public string? DiskSize { get; set; }

    [BooleanCommandSwitch("--enable-high-resilience")]
    public bool? EnableHighResilience { get; set; }

    [BooleanCommandSwitch("--enable-logs-in-cloud-logging-only")]
    public bool? EnableLogsInCloudLoggingOnly { get; set; }

    [CommandSwitch("--env-variables")]
    public string[]? EnvVariables { get; set; }

    [CommandSwitch("--environment-size")]
    public string? EnvironmentSize { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--node-count")]
    public string? NodeCount { get; set; }

    [CommandSwitch("--oauth-scopes")]
    public string[]? OauthScopes { get; set; }

    [CommandSwitch("--python-version")]
    public string? PythonVersion { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--storage-bucket")]
    public string? StorageBucket { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--web-server-machine-type")]
    public string? WebServerMachineType { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--airflow-version")]
    public string? AirflowVersion { get; set; }

    [CommandSwitch("--image-version")]
    public string? ImageVersion { get; set; }

    [CommandSwitch("--cloud-sql-ipv4-cidr")]
    public string? CloudSqlIpv4Cidr { get; set; }

    [CommandSwitch("--composer-network-ipv4-cidr")]
    public string? ComposerNetworkIpv4Cidr { get; set; }

    [CommandSwitch("--connection-subnetwork")]
    public string? ConnectionSubnetwork { get; set; }

    [CommandSwitch("--connection-type")]
    public string? ConnectionType { get; set; }

    [BooleanCommandSwitch("--enable-private-endpoint")]
    public bool? EnablePrivateEndpoint { get; set; }

    [BooleanCommandSwitch("--enable-private-environment")]
    public bool? EnablePrivateEnvironment { get; set; }

    [BooleanCommandSwitch("--enable-privately-used-public-ips")]
    public bool? EnablePrivatelyUsedPublicIps { get; set; }

    [CommandSwitch("--master-ipv4-cidr")]
    public string? MasterIpv4Cidr { get; set; }

    [CommandSwitch("--web-server-ipv4-cidr")]
    public string? WebServerIpv4Cidr { get; set; }

    [CommandSwitch("--cluster-ipv4-cidr")]
    public string? ClusterIpv4Cidr { get; set; }

    [CommandSwitch("--cluster-secondary-range-name")]
    public string? ClusterSecondaryRangeName { get; set; }

    [BooleanCommandSwitch("--enable-ip-alias")]
    public bool? EnableIpAlias { get; set; }

    [BooleanCommandSwitch("--enable-ip-masq-agent")]
    public bool? EnableIpMasqAgent { get; set; }

    [CommandSwitch("--services-ipv4-cidr")]
    public string? ServicesIpv4Cidr { get; set; }

    [CommandSwitch("--services-secondary-range-name")]
    public string? ServicesSecondaryRangeName { get; set; }

    [BooleanCommandSwitch("--enable-master-authorized-networks")]
    public bool? EnableMasterAuthorizedNetworks { get; set; }

    [CommandSwitch("--master-authorized-networks")]
    public string[]? MasterAuthorizedNetworks { get; set; }

    [BooleanCommandSwitch("--enable-scheduled-snapshot-creation")]
    public bool? EnableScheduledSnapshotCreation { get; set; }

    [CommandSwitch("--snapshot-creation-schedule")]
    public string? SnapshotCreationSchedule { get; set; }

    [CommandSwitch("--snapshot-location")]
    public string? SnapshotLocation { get; set; }

    [CommandSwitch("--snapshot-schedule-timezone")]
    public string? SnapshotScheduleTimezone { get; set; }

    [BooleanCommandSwitch("--enable-triggerer")]
    public bool? EnableTriggerer { get; set; }

    [CommandSwitch("--triggerer-count")]
    public string? TriggererCount { get; set; }

    [CommandSwitch("--triggerer-cpu")]
    public string? TriggererCpu { get; set; }

    [CommandSwitch("--triggerer-memory")]
    public string? TriggererMemory { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }

    [CommandSwitch("--maintenance-window-end")]
    public string? MaintenanceWindowEnd { get; set; }

    [CommandSwitch("--maintenance-window-recurrence")]
    public string? MaintenanceWindowRecurrence { get; set; }

    [CommandSwitch("--maintenance-window-start")]
    public string? MaintenanceWindowStart { get; set; }

    [CommandSwitch("--max-workers")]
    public string? MaxWorkers { get; set; }

    [CommandSwitch("--min-workers")]
    public string? MinWorkers { get; set; }

    [CommandSwitch("--scheduler-count")]
    public string? SchedulerCount { get; set; }

    [CommandSwitch("--scheduler-cpu")]
    public string? SchedulerCpu { get; set; }

    [CommandSwitch("--scheduler-memory")]
    public string? SchedulerMemory { get; set; }

    [CommandSwitch("--scheduler-storage")]
    public string? SchedulerStorage { get; set; }

    [CommandSwitch("--web-server-cpu")]
    public string? WebServerCpu { get; set; }

    [CommandSwitch("--web-server-memory")]
    public string? WebServerMemory { get; set; }

    [CommandSwitch("--web-server-storage")]
    public string? WebServerStorage { get; set; }

    [CommandSwitch("--worker-cpu")]
    public string? WorkerCpu { get; set; }

    [CommandSwitch("--worker-memory")]
    public string? WorkerMemory { get; set; }

    [CommandSwitch("--worker-storage")]
    public string? WorkerStorage { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [BooleanCommandSwitch("--web-server-allow-all")]
    public bool? WebServerAllowAll { get; set; }

    [CommandSwitch("--web-server-allow-ip")]
    public string[]? WebServerAllowIp { get; set; }

    [BooleanCommandSwitch("ip_range")]
    public bool? IpRange { get; set; }

    [BooleanCommandSwitch("description")]
    public bool? Description { get; set; }

    [BooleanCommandSwitch("--web-server-deny-all")]
    public bool? WebServerDenyAll { get; set; }
}