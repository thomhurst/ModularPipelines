using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight-on-aks", "cluster", "update")]
public record AzHdinsightOnAksClusterUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--application-log-std-error-enabled")]
    public bool? ApplicationLogStdErrorEnabled { get; set; }

    [BooleanCommandSwitch("--application-log-std-out-enabled")]
    public bool? ApplicationLogStdOutEnabled { get; set; }

    [CommandSwitch("--assigned-identity-client-id")]
    public string? AssignedIdentityClientId { get; set; }

    [CommandSwitch("--assigned-identity-id")]
    public string? AssignedIdentityId { get; set; }

    [CommandSwitch("--assigned-identity-object-id")]
    public string? AssignedIdentityObjectId { get; set; }

    [CommandSwitch("--authorization-group-id")]
    public string? AuthorizationGroupId { get; set; }

    [CommandSwitch("--authorization-user-id")]
    public string? AuthorizationUserId { get; set; }

    [CommandSwitch("--autoscale-profile-graceful-decommission-timeout")]
    public string? AutoscaleProfileGracefulDecommissionTimeout { get; set; }

    [CommandSwitch("--autoscale-profile-type")]
    public string? AutoscaleProfileType { get; set; }

    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--cluster-pool-name")]
    public string? ClusterPoolName { get; set; }

    [CommandSwitch("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CommandSwitch("--cooldown-period")]
    public string? CooldownPeriod { get; set; }

    [CommandSwitch("--coord-debug-port")]
    public string? CoordDebugPort { get; set; }

    [BooleanCommandSwitch("--coord-debug-suspend")]
    public bool? CoordDebugSuspend { get; set; }

    [BooleanCommandSwitch("--coordinator-debug-enabled")]
    public bool? CoordinatorDebugEnabled { get; set; }

    [BooleanCommandSwitch("--coordinator-high-availability-enabled")]
    public bool? CoordinatorHighAvailabilityEnabled { get; set; }

    [BooleanCommandSwitch("--enable-autoscale")]
    public bool? EnableAutoscale { get; set; }

    [BooleanCommandSwitch("--enable-la-metrics")]
    public bool? EnableLaMetrics { get; set; }

    [BooleanCommandSwitch("--enable-log-analytics")]
    public bool? EnableLogAnalytics { get; set; }

    [BooleanCommandSwitch("--enable-prometheu")]
    public bool? EnablePrometheu { get; set; }

    [BooleanCommandSwitch("--enable-worker-debug")]
    public bool? EnableWorkerDebug { get; set; }

    [CommandSwitch("--flink-hive-catalog-db-connection-password-secret")]
    public string? FlinkHiveCatalogDbConnectionPasswordSecret { get; set; }

    [CommandSwitch("--flink-hive-catalog-db-connection-url")]
    public string? FlinkHiveCatalogDbConnectionUrl { get; set; }

    [CommandSwitch("--flink-hive-catalog-db-connection-user-name")]
    public string? FlinkHiveCatalogDbConnectionUserName { get; set; }

    [CommandSwitch("--flink-storage-key")]
    public string? FlinkStorageKey { get; set; }

    [CommandSwitch("--flink-storage-uri")]
    public string? FlinkStorageUri { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--history-server-cpu")]
    public string? HistoryServerCpu { get; set; }

    [CommandSwitch("--history-server-memory")]
    public string? HistoryServerMemory { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--job-manager-cpu")]
    public string? JobManagerCpu { get; set; }

    [CommandSwitch("--job-manager-memory")]
    public string? JobManagerMemory { get; set; }

    [CommandSwitch("--kafka-profile")]
    public string? KafkaProfile { get; set; }

    [CommandSwitch("--key-vault-id")]
    public string? KeyVaultId { get; set; }

    [CommandSwitch("--llap-profile")]
    public string? LlapProfile { get; set; }

    [CommandSwitch("--loadbased-config-max-nodes")]
    public string? LoadbasedConfigMaxNodes { get; set; }

    [CommandSwitch("--loadbased-config-min-nodes")]
    public string? LoadbasedConfigMinNodes { get; set; }

    [CommandSwitch("--loadbased-config-poll-interval")]
    public string? LoadbasedConfigPollInterval { get; set; }

    [CommandSwitch("--loadbased-config-scaling-rules")]
    public string? LoadbasedConfigScalingRules { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nodes")]
    public string? Nodes { get; set; }

    [CommandSwitch("--num-replicas")]
    public string? NumReplicas { get; set; }

    [CommandSwitch("--oss-version")]
    public string? OssVersion { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--schedule-based-config-default-count")]
    public int? ScheduleBasedConfigDefaultCount { get; set; }

    [CommandSwitch("--schedule-based-config-schedule")]
    public string? ScheduleBasedConfigSchedule { get; set; }

    [CommandSwitch("--schedule-based-config-time-zone")]
    public string? ScheduleBasedConfigTimeZone { get; set; }

    [CommandSwitch("--script-action-profiles")]
    public string? ScriptActionProfiles { get; set; }

    [CommandSwitch("--secret-reference")]
    public string? SecretReference { get; set; }

    [CommandSwitch("--service-configs")]
    public string? ServiceConfigs { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--spark-hive-catalog-db-name")]
    public string? SparkHiveCatalogDbName { get; set; }

    [CommandSwitch("--spark-hive-catalog-db-password-secret")]
    public string? SparkHiveCatalogDbPasswordSecret { get; set; }

    [CommandSwitch("--spark-hive-catalog-db-server-name")]
    public string? SparkHiveCatalogDbServerName { get; set; }

    [CommandSwitch("--spark-hive-catalog-db-user-name")]
    public string? SparkHiveCatalogDbUserName { get; set; }

    [CommandSwitch("--spark-hive-catalog-key-vault-id")]
    public string? SparkHiveCatalogKeyVaultId { get; set; }

    [CommandSwitch("--spark-hive-catalog-thrift-url")]
    public string? SparkHiveCatalogThriftUrl { get; set; }

    [CommandSwitch("--spark-storage-url")]
    public string? SparkStorageUrl { get; set; }

    [CommandSwitch("--ssh-profile-count")]
    public int? SshProfileCount { get; set; }

    [CommandSwitch("--stub-profile")]
    public string? StubProfile { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--task-manager-cpu")]
    public string? TaskManagerCpu { get; set; }

    [CommandSwitch("--task-manager-memory")]
    public string? TaskManagerMemory { get; set; }

    [CommandSwitch("--trino-hive-catalog")]
    public string? TrinoHiveCatalog { get; set; }

    [CommandSwitch("--trino-plugins-spec")]
    public string? TrinoPluginsSpec { get; set; }

    [CommandSwitch("--trino-profile-user-plugins-telemetry-spec")]
    public string? TrinoProfileUserPluginsTelemetrySpec { get; set; }

    [CommandSwitch("--user-plugins-spec")]
    public string? UserPluginsSpec { get; set; }

    [CommandSwitch("--worker-debug-port")]
    public string? WorkerDebugPort { get; set; }

    [BooleanCommandSwitch("--worker-debug-suspend")]
    public bool? WorkerDebugSuspend { get; set; }
}

