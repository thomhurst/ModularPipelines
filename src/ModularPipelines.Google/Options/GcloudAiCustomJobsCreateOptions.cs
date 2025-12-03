using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "custom-jobs", "create")]
public record GcloudAiCustomJobsCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--config")] string Config,
[property: CliOption("--worker-pool-spec")] string[] WorkerPoolSpec,
[property: CliFlag("machine-type")] bool MachineType,
[property: CliFlag("replica-count")] bool ReplicaCount,
[property: CliFlag("accelerator-type")] bool AcceleratorType,
[property: CliFlag("accelerator-count")] bool AcceleratorCount,
[property: CliFlag("container-image-uri")] bool ContainerImageUri,
[property: CliFlag("executor-image-uri")] bool ExecutorImageUri,
[property: CliFlag("output-image-uri")] bool OutputImageUri,
[property: CliFlag("python-module")] bool PythonModule,
[property: CliFlag("local-package-path")] bool LocalPackagePath,
[property: CliFlag("script")] bool Script,
[property: CliFlag("requirements")] bool Requirements,
[property: CliFlag("extra-packages")] bool ExtraPackages,
[property: CliFlag("extra-dirs")] bool ExtraDirs
) : GcloudOptions
{
    [CliOption("--args")]
    public string[]? Args { get; set; }

    [CliOption("--command")]
    public string[]? Command { get; set; }

    [CliFlag("--enable-dashboard-access")]
    public bool? EnableDashboardAccess { get; set; }

    [CliFlag("--enable-web-access")]
    public bool? EnableWebAccess { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--python-package-uris")]
    public string[]? PythonPackageUris { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}