using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight-on-aks", "cluster", "update")]
public record AzHdinsightOnAksClusterUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--application-log-std-error-enabled")]
    public bool? ApplicationLogStdErrorEnabled { get; set; }

    [CliFlag("--application-log-std-out-enabled")]
    public bool? ApplicationLogStdOutEnabled { get; set; }

    [CliOption("--assigned-identity-client-id")]
    public string? AssignedIdentityClientId { get; set; }

    [CliOption("--assigned-identity-id")]
    public string? AssignedIdentityId { get; set; }

    [CliOption("--assigned-identity-object-id")]
    public string? AssignedIdentityObjectId { get; set; }

    [CliOption("--authorization-group-id")]
    public string? AuthorizationGroupId { get; set; }

    [CliOption("--authorization-user-id")]
    public string? AuthorizationUserId { get; set; }

    [CliOption("--autoscale-profile-graceful-decommission-timeout")]
    public string? AutoscaleProfileGracefulDecommissionTimeout { get; set; }

    [CliOption("--autoscale-profile-type")]
    public string? AutoscaleProfileType { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--cluster-pool-name")]
    public string? ClusterPoolName { get; set; }

    [CliOption("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CliOption("--cooldown-period")]
    public string? CooldownPeriod { get; set; }

    [CliOption("--coord-debug-port")]
    public string? CoordDebugPort { get; set; }

    [CliFlag("--coord-debug-suspend")]
    public bool? CoordDebugSuspend { get; set; }

    [CliFlag("--coordinator-debug-enabled")]
    public bool? CoordinatorDebugEnabled { get; set; }

    [CliFlag("--coordinator-high-availability-enabled")]
    public bool? CoordinatorHighAvailabilityEnabled { get; set; }

    [CliFlag("--enable-autoscale")]
    public bool? EnableAutoscale { get; set; }

    [CliFlag("--enable-la-metrics")]
    public bool? EnableLaMetrics { get; set; }

    [CliFlag("--enable-log-analytics")]
    public bool? EnableLogAnalytics { get; set; }

    [CliFlag("--enable-prometheu")]
    public bool? EnablePrometheu { get; set; }

    [CliFlag("--enable-worker-debug")]
    public bool? EnableWorkerDebug { get; set; }

    [CliOption("--flink-hive-catalog-db-connection-password-secret")]
    public string? FlinkHiveCatalogDbConnectionPasswordSecret { get; set; }

    [CliOption("--flink-hive-catalog-db-connection-url")]
    public string? FlinkHiveCatalogDbConnectionUrl { get; set; }

    [CliOption("--flink-hive-catalog-db-connection-user-name")]
    public string? FlinkHiveCatalogDbConnectionUserName { get; set; }

    [CliOption("--flink-storage-key")]
    public string? FlinkStorageKey { get; set; }

    [CliOption("--flink-storage-uri")]
    public string? FlinkStorageUri { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--history-server-cpu")]
    public string? HistoryServerCpu { get; set; }

    [CliOption("--history-server-memory")]
    public string? HistoryServerMemory { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--job-manager-cpu")]
    public string? JobManagerCpu { get; set; }

    [CliOption("--job-manager-memory")]
    public string? JobManagerMemory { get; set; }

    [CliOption("--kafka-profile")]
    public string? KafkaProfile { get; set; }

    [CliOption("--key-vault-id")]
    public string? KeyVaultId { get; set; }

    [CliOption("--llap-profile")]
    public string? LlapProfile { get; set; }

    [CliOption("--loadbased-config-max-nodes")]
    public string? LoadbasedConfigMaxNodes { get; set; }

    [CliOption("--loadbased-config-min-nodes")]
    public string? LoadbasedConfigMinNodes { get; set; }

    [CliOption("--loadbased-config-poll-interval")]
    public string? LoadbasedConfigPollInterval { get; set; }

    [CliOption("--loadbased-config-scaling-rules")]
    public string? LoadbasedConfigScalingRules { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nodes")]
    public string? Nodes { get; set; }

    [CliOption("--num-replicas")]
    public string? NumReplicas { get; set; }

    [CliOption("--oss-version")]
    public string? OssVersion { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--schedule-based-config-default-count")]
    public int? ScheduleBasedConfigDefaultCount { get; set; }

    [CliOption("--schedule-based-config-schedule")]
    public string? ScheduleBasedConfigSchedule { get; set; }

    [CliOption("--schedule-based-config-time-zone")]
    public string? ScheduleBasedConfigTimeZone { get; set; }

    [CliOption("--script-action-profiles")]
    public string? ScriptActionProfiles { get; set; }

    [CliOption("--secret-reference")]
    public string? SecretReference { get; set; }

    [CliOption("--service-configs")]
    public string? ServiceConfigs { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--spark-hive-catalog-db-name")]
    public string? SparkHiveCatalogDbName { get; set; }

    [CliOption("--spark-hive-catalog-db-password-secret")]
    public string? SparkHiveCatalogDbPasswordSecret { get; set; }

    [CliOption("--spark-hive-catalog-db-server-name")]
    public string? SparkHiveCatalogDbServerName { get; set; }

    [CliOption("--spark-hive-catalog-db-user-name")]
    public string? SparkHiveCatalogDbUserName { get; set; }

    [CliOption("--spark-hive-catalog-key-vault-id")]
    public string? SparkHiveCatalogKeyVaultId { get; set; }

    [CliOption("--spark-hive-catalog-thrift-url")]
    public string? SparkHiveCatalogThriftUrl { get; set; }

    [CliOption("--spark-storage-url")]
    public string? SparkStorageUrl { get; set; }

    [CliOption("--ssh-profile-count")]
    public int? SshProfileCount { get; set; }

    [CliOption("--stub-profile")]
    public string? StubProfile { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--task-manager-cpu")]
    public string? TaskManagerCpu { get; set; }

    [CliOption("--task-manager-memory")]
    public string? TaskManagerMemory { get; set; }

    [CliOption("--trino-hive-catalog")]
    public string? TrinoHiveCatalog { get; set; }

    [CliOption("--trino-plugins-spec")]
    public string? TrinoPluginsSpec { get; set; }

    [CliOption("--trino-profile-user-plugins-telemetry-spec")]
    public string? TrinoProfileUserPluginsTelemetrySpec { get; set; }

    [CliOption("--user-plugins-spec")]
    public string? UserPluginsSpec { get; set; }

    [CliOption("--worker-debug-port")]
    public string? WorkerDebugPort { get; set; }

    [CliFlag("--worker-debug-suspend")]
    public bool? WorkerDebugSuspend { get; set; }
}