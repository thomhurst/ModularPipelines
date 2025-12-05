using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "update")]
public record GcloudComposerEnvironmentsUpdateOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location,
[property: CliOption("--cloud-sql-machine-type")] string CloudSqlMachineType,
[property: CliFlag("--disable-high-resilience")] bool DisableHighResilience,
[property: CliFlag("--disable-logs-in-cloud-logging-only")] bool DisableLogsInCloudLoggingOnly,
[property: CliFlag("--enable-high-resilience")] bool EnableHighResilience,
[property: CliFlag("--enable-logs-in-cloud-logging-only")] bool EnableLogsInCloudLoggingOnly,
[property: CliOption("--environment-size")] string EnvironmentSize,
[property: CliOption("--node-count")] string NodeCount,
[property: CliOption("--web-server-machine-type")] string WebServerMachineType,
[property: CliFlag("--disable-master-authorized-networks")] bool DisableMasterAuthorizedNetworks,
[property: CliFlag("--enable-master-authorized-networks")] bool EnableMasterAuthorizedNetworks,
[property: CliOption("--master-authorized-networks")] string[] MasterAuthorizedNetworks,
[property: CliFlag("--disable-scheduled-snapshot-creation")] bool DisableScheduledSnapshotCreation,
[property: CliFlag("--enable-scheduled-snapshot-creation")] bool EnableScheduledSnapshotCreation,
[property: CliOption("--snapshot-creation-schedule")] string SnapshotCreationSchedule,
[property: CliOption("--snapshot-location")] string SnapshotLocation,
[property: CliOption("--snapshot-schedule-timezone")] string SnapshotScheduleTimezone,
[property: CliOption("--maintenance-window-end")] string MaintenanceWindowEnd,
[property: CliOption("--maintenance-window-recurrence")] string MaintenanceWindowRecurrence,
[property: CliOption("--maintenance-window-start")] string MaintenanceWindowStart,
[property: CliOption("--max-workers")] string MaxWorkers,
[property: CliOption("--min-workers")] string MinWorkers,
[property: CliOption("--scheduler-count")] string SchedulerCount,
[property: CliOption("--scheduler-cpu")] string SchedulerCpu,
[property: CliOption("--scheduler-memory")] string SchedulerMemory,
[property: CliOption("--scheduler-storage")] string SchedulerStorage,
[property: CliOption("--web-server-cpu")] string WebServerCpu,
[property: CliOption("--web-server-memory")] string WebServerMemory,
[property: CliOption("--web-server-storage")] string WebServerStorage,
[property: CliOption("--worker-cpu")] string WorkerCpu,
[property: CliOption("--worker-memory")] string WorkerMemory,
[property: CliOption("--worker-storage")] string WorkerStorage,
[property: CliFlag("--disable-triggerer")] bool DisableTriggerer,
[property: CliFlag("--enable-triggerer")] bool EnableTriggerer,
[property: CliOption("--triggerer-count")] string TriggererCount,
[property: CliOption("--triggerer-cpu")] string TriggererCpu,
[property: CliOption("--triggerer-memory")] string TriggererMemory,
[property: CliOption("--update-airflow-configs")] IEnumerable<KeyValue> UpdateAirflowConfigs,
[property: CliFlag("--clear-airflow-configs")] bool ClearAirflowConfigs,
[property: CliOption("--remove-airflow-configs")] string[] RemoveAirflowConfigs,
[property: CliOption("--update-env-variables")] string[] UpdateEnvVariables,
[property: CliFlag("--clear-env-variables")] bool ClearEnvVariables,
[property: CliOption("--remove-env-variables")] string[] RemoveEnvVariables,
[property: CliOption("--update-labels")] IEnumerable<KeyValue> UpdateLabels,
[property: CliFlag("--clear-labels")] bool ClearLabels,
[property: CliOption("--remove-labels")] string[] RemoveLabels,
[property: CliOption("--update-pypi-packages-from-file")] string UpdatePypiPackagesFromFile,
[property: CliOption("--update-pypi-package")] string[] UpdatePypiPackage,
[property: CliFlag("--clear-pypi-packages")] bool ClearPypiPackages,
[property: CliOption("--remove-pypi-packages")] string[] RemovePypiPackages,
[property: CliOption("--update-web-server-allow-ip")] string[] UpdateWebServerAllowIp,
[property: CliFlag("ip_range")] bool IpRange,
[property: CliFlag("description")] bool Description,
[property: CliFlag("--web-server-allow-all")] bool WebServerAllowAll,
[property: CliFlag("--web-server-deny-all")] bool WebServerDenyAll
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}