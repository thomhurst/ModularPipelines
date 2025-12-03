using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "create")]
public record GcloudComposerEnvironmentsCreateOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--airflow-configs")]
    public IEnumerable<KeyValue>? AirflowConfigs { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cloud-sql-machine-type")]
    public string? CloudSqlMachineType { get; set; }

    [CliOption("--cloud-sql-preferred-zone")]
    public string? CloudSqlPreferredZone { get; set; }

    [CliFlag("--disable-logs-in-cloud-logging-only")]
    public bool? DisableLogsInCloudLoggingOnly { get; set; }

    [CliOption("--disk-size")]
    public string? DiskSize { get; set; }

    [CliFlag("--enable-high-resilience")]
    public bool? EnableHighResilience { get; set; }

    [CliFlag("--enable-logs-in-cloud-logging-only")]
    public bool? EnableLogsInCloudLoggingOnly { get; set; }

    [CliOption("--env-variables")]
    public string[]? EnvVariables { get; set; }

    [CliOption("--environment-size")]
    public string? EnvironmentSize { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--node-count")]
    public string? NodeCount { get; set; }

    [CliOption("--oauth-scopes")]
    public string[]? OauthScopes { get; set; }

    [CliOption("--python-version")]
    public string? PythonVersion { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--storage-bucket")]
    public string? StorageBucket { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--web-server-machine-type")]
    public string? WebServerMachineType { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--airflow-version")]
    public string? AirflowVersion { get; set; }

    [CliOption("--image-version")]
    public string? ImageVersion { get; set; }

    [CliOption("--cloud-sql-ipv4-cidr")]
    public string? CloudSqlIpv4Cidr { get; set; }

    [CliOption("--composer-network-ipv4-cidr")]
    public string? ComposerNetworkIpv4Cidr { get; set; }

    [CliOption("--connection-subnetwork")]
    public string? ConnectionSubnetwork { get; set; }

    [CliOption("--connection-type")]
    public string? ConnectionType { get; set; }

    [CliFlag("--enable-private-endpoint")]
    public bool? EnablePrivateEndpoint { get; set; }

    [CliFlag("--enable-private-environment")]
    public bool? EnablePrivateEnvironment { get; set; }

    [CliFlag("--enable-privately-used-public-ips")]
    public bool? EnablePrivatelyUsedPublicIps { get; set; }

    [CliOption("--master-ipv4-cidr")]
    public string? MasterIpv4Cidr { get; set; }

    [CliOption("--web-server-ipv4-cidr")]
    public string? WebServerIpv4Cidr { get; set; }

    [CliOption("--cluster-ipv4-cidr")]
    public string? ClusterIpv4Cidr { get; set; }

    [CliOption("--cluster-secondary-range-name")]
    public string? ClusterSecondaryRangeName { get; set; }

    [CliFlag("--enable-ip-alias")]
    public bool? EnableIpAlias { get; set; }

    [CliFlag("--enable-ip-masq-agent")]
    public bool? EnableIpMasqAgent { get; set; }

    [CliOption("--services-ipv4-cidr")]
    public string? ServicesIpv4Cidr { get; set; }

    [CliOption("--services-secondary-range-name")]
    public string? ServicesSecondaryRangeName { get; set; }

    [CliFlag("--enable-master-authorized-networks")]
    public bool? EnableMasterAuthorizedNetworks { get; set; }

    [CliOption("--master-authorized-networks")]
    public string[]? MasterAuthorizedNetworks { get; set; }

    [CliFlag("--enable-scheduled-snapshot-creation")]
    public bool? EnableScheduledSnapshotCreation { get; set; }

    [CliOption("--snapshot-creation-schedule")]
    public string? SnapshotCreationSchedule { get; set; }

    [CliOption("--snapshot-location")]
    public string? SnapshotLocation { get; set; }

    [CliOption("--snapshot-schedule-timezone")]
    public string? SnapshotScheduleTimezone { get; set; }

    [CliFlag("--enable-triggerer")]
    public bool? EnableTriggerer { get; set; }

    [CliOption("--triggerer-count")]
    public string? TriggererCount { get; set; }

    [CliOption("--triggerer-cpu")]
    public string? TriggererCpu { get; set; }

    [CliOption("--triggerer-memory")]
    public string? TriggererMemory { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }

    [CliOption("--maintenance-window-end")]
    public string? MaintenanceWindowEnd { get; set; }

    [CliOption("--maintenance-window-recurrence")]
    public string? MaintenanceWindowRecurrence { get; set; }

    [CliOption("--maintenance-window-start")]
    public string? MaintenanceWindowStart { get; set; }

    [CliOption("--max-workers")]
    public string? MaxWorkers { get; set; }

    [CliOption("--min-workers")]
    public string? MinWorkers { get; set; }

    [CliOption("--scheduler-count")]
    public string? SchedulerCount { get; set; }

    [CliOption("--scheduler-cpu")]
    public string? SchedulerCpu { get; set; }

    [CliOption("--scheduler-memory")]
    public string? SchedulerMemory { get; set; }

    [CliOption("--scheduler-storage")]
    public string? SchedulerStorage { get; set; }

    [CliOption("--web-server-cpu")]
    public string? WebServerCpu { get; set; }

    [CliOption("--web-server-memory")]
    public string? WebServerMemory { get; set; }

    [CliOption("--web-server-storage")]
    public string? WebServerStorage { get; set; }

    [CliOption("--worker-cpu")]
    public string? WorkerCpu { get; set; }

    [CliOption("--worker-memory")]
    public string? WorkerMemory { get; set; }

    [CliOption("--worker-storage")]
    public string? WorkerStorage { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliFlag("--web-server-allow-all")]
    public bool? WebServerAllowAll { get; set; }

    [CliOption("--web-server-allow-ip")]
    public string[]? WebServerAllowIp { get; set; }

    [CliFlag("ip_range")]
    public bool? IpRange { get; set; }

    [CliFlag("description")]
    public bool? Description { get; set; }

    [CliFlag("--web-server-deny-all")]
    public bool? WebServerDenyAll { get; set; }
}