using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "custom-jobs", "create")]
public record GcloudAiCustomJobsCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--worker-pool-spec")] string[] WorkerPoolSpec,
[property: BooleanCommandSwitch("machine-type")] bool MachineType,
[property: BooleanCommandSwitch("replica-count")] bool ReplicaCount,
[property: BooleanCommandSwitch("accelerator-type")] bool AcceleratorType,
[property: BooleanCommandSwitch("accelerator-count")] bool AcceleratorCount,
[property: BooleanCommandSwitch("container-image-uri")] bool ContainerImageUri,
[property: BooleanCommandSwitch("executor-image-uri")] bool ExecutorImageUri,
[property: BooleanCommandSwitch("output-image-uri")] bool OutputImageUri,
[property: BooleanCommandSwitch("python-module")] bool PythonModule,
[property: BooleanCommandSwitch("local-package-path")] bool LocalPackagePath,
[property: BooleanCommandSwitch("script")] bool Script,
[property: BooleanCommandSwitch("requirements")] bool Requirements,
[property: BooleanCommandSwitch("extra-packages")] bool ExtraPackages,
[property: BooleanCommandSwitch("extra-dirs")] bool ExtraDirs
) : GcloudOptions
{
    [CommandSwitch("--args")]
    public string[]? Args { get; set; }

    [CommandSwitch("--command")]
    public string[]? Command { get; set; }

    [BooleanCommandSwitch("--enable-dashboard-access")]
    public bool? EnableDashboardAccess { get; set; }

    [BooleanCommandSwitch("--enable-web-access")]
    public bool? EnableWebAccess { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--python-package-uris")]
    public string[]? PythonPackageUris { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}