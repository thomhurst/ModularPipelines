using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "snapshots", "load")]
public record GcloudComposerEnvironmentsSnapshotsLoadOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location,
[property: CliOption("--snapshot-path")] string SnapshotPath
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--skip-airflow-overrides-setting")]
    public bool? SkipAirflowOverridesSetting { get; set; }

    [CliFlag("--skip-environment-variables-setting")]
    public bool? SkipEnvironmentVariablesSetting { get; set; }

    [CliFlag("--skip-gcs-data-copying")]
    public bool? SkipGcsDataCopying { get; set; }

    [CliFlag("--skip-pypi-packages-installation")]
    public bool? SkipPypiPackagesInstallation { get; set; }
}