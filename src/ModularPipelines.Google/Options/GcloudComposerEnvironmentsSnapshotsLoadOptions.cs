using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "snapshots", "load")]
public record GcloudComposerEnvironmentsSnapshotsLoadOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--snapshot-path")] string SnapshotPath
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--skip-airflow-overrides-setting")]
    public bool? SkipAirflowOverridesSetting { get; set; }

    [BooleanCommandSwitch("--skip-environment-variables-setting")]
    public bool? SkipEnvironmentVariablesSetting { get; set; }

    [BooleanCommandSwitch("--skip-gcs-data-copying")]
    public bool? SkipGcsDataCopying { get; set; }

    [BooleanCommandSwitch("--skip-pypi-packages-installation")]
    public bool? SkipPypiPackagesInstallation { get; set; }
}