using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "update")]
public record GcloudComposerEnvironmentsUpdateOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--cloud-sql-machine-type")] string CloudSqlMachineType,
[property: BooleanCommandSwitch("--disable-high-resilience")] bool DisableHighResilience,
[property: BooleanCommandSwitch("--disable-logs-in-cloud-logging-only")] bool DisableLogsInCloudLoggingOnly,
[property: BooleanCommandSwitch("--enable-high-resilience")] bool EnableHighResilience,
[property: BooleanCommandSwitch("--enable-logs-in-cloud-logging-only")] bool EnableLogsInCloudLoggingOnly,
[property: CommandSwitch("--environment-size")] string EnvironmentSize,
[property: CommandSwitch("--node-count")] string NodeCount,
[property: CommandSwitch("--web-server-machine-type")] string WebServerMachineType,
[property: BooleanCommandSwitch("--disable-master-authorized-networks")] bool DisableMasterAuthorizedNetworks,
[property: BooleanCommandSwitch("--enable-master-authorized-networks")] bool EnableMasterAuthorizedNetworks,
[property: CommandSwitch("--master-authorized-networks")] string[] MasterAuthorizedNetworks,
[property: BooleanCommandSwitch("--disable-scheduled-snapshot-creation")] bool DisableScheduledSnapshotCreation,
[property: BooleanCommandSwitch("--enable-scheduled-snapshot-creation")] bool EnableScheduledSnapshotCreation,
[property: CommandSwitch("--snapshot-creation-schedule")] string SnapshotCreationSchedule,
[property: CommandSwitch("--snapshot-location")] string SnapshotLocation,
[property: CommandSwitch("--snapshot-schedule-timezone")] string SnapshotScheduleTimezone,
[property: CommandSwitch("--maintenance-window-end")] string MaintenanceWindowEnd,
[property: CommandSwitch("--maintenance-window-recurrence")] string MaintenanceWindowRecurrence,
[property: CommandSwitch("--maintenance-window-start")] string MaintenanceWindowStart,
[property: CommandSwitch("--max-workers")] string MaxWorkers,
[property: CommandSwitch("--min-workers")] string MinWorkers,
[property: CommandSwitch("--scheduler-count")] string SchedulerCount,
[property: CommandSwitch("--scheduler-cpu")] string SchedulerCpu,
[property: CommandSwitch("--scheduler-memory")] string SchedulerMemory,
[property: CommandSwitch("--scheduler-storage")] string SchedulerStorage,
[property: CommandSwitch("--web-server-cpu")] string WebServerCpu,
[property: CommandSwitch("--web-server-memory")] string WebServerMemory,
[property: CommandSwitch("--web-server-storage")] string WebServerStorage,
[property: CommandSwitch("--worker-cpu")] string WorkerCpu,
[property: CommandSwitch("--worker-memory")] string WorkerMemory,
[property: CommandSwitch("--worker-storage")] string WorkerStorage,
[property: BooleanCommandSwitch("--disable-triggerer")] bool DisableTriggerer,
[property: BooleanCommandSwitch("--enable-triggerer")] bool EnableTriggerer,
[property: CommandSwitch("--triggerer-count")] string TriggererCount,
[property: CommandSwitch("--triggerer-cpu")] string TriggererCpu,
[property: CommandSwitch("--triggerer-memory")] string TriggererMemory,
[property: CommandSwitch("--update-airflow-configs")] IEnumerable<KeyValue> UpdateAirflowConfigs,
[property: BooleanCommandSwitch("--clear-airflow-configs")] bool ClearAirflowConfigs,
[property: CommandSwitch("--remove-airflow-configs")] string[] RemoveAirflowConfigs,
[property: CommandSwitch("--update-env-variables")] string[] UpdateEnvVariables,
[property: BooleanCommandSwitch("--clear-env-variables")] bool ClearEnvVariables,
[property: CommandSwitch("--remove-env-variables")] string[] RemoveEnvVariables,
[property: CommandSwitch("--update-labels")] IEnumerable<KeyValue> UpdateLabels,
[property: BooleanCommandSwitch("--clear-labels")] bool ClearLabels,
[property: CommandSwitch("--remove-labels")] string[] RemoveLabels,
[property: CommandSwitch("--update-pypi-packages-from-file")] string UpdatePypiPackagesFromFile,
[property: CommandSwitch("--update-pypi-package")] string[] UpdatePypiPackage,
[property: BooleanCommandSwitch("--clear-pypi-packages")] bool ClearPypiPackages,
[property: CommandSwitch("--remove-pypi-packages")] string[] RemovePypiPackages,
[property: CommandSwitch("--update-web-server-allow-ip")] string[] UpdateWebServerAllowIp,
[property: BooleanCommandSwitch("ip_range")] bool IpRange,
[property: BooleanCommandSwitch("description")] bool Description,
[property: BooleanCommandSwitch("--web-server-allow-all")] bool WebServerAllowAll,
[property: BooleanCommandSwitch("--web-server-deny-all")] bool WebServerDenyAll
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}