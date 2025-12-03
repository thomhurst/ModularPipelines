using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "jobs", "submit", "training")]
public record GcloudMlEngineJobsSubmitTrainingOptions(
[property: CliArgument] string Job,
[property: CliArgument] string UserArgs
) : GcloudOptions
{
    [CliOption("--config")]
    public string? Config { get; set; }

    [CliFlag("--enable-web-access")]
    public bool? EnableWebAccess { get; set; }

    [CliOption("--job-dir")]
    public string? JobDir { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--master-accelerator")]
    public string[]? MasterAccelerator { get; set; }

    [CliOption("--master-image-uri")]
    public string? MasterImageUri { get; set; }

    [CliOption("--master-machine-type")]
    public string? MasterMachineType { get; set; }

    [CliOption("--module-name")]
    public string? ModuleName { get; set; }

    [CliOption("--package-path")]
    public string? PackagePath { get; set; }

    [CliOption("--packages")]
    public string[]? Packages { get; set; }

    [CliOption("--parameter-server-accelerator")]
    public string[]? ParameterServerAccelerator { get; set; }

    [CliOption("--parameter-server-image-uri")]
    public string? ParameterServerImageUri { get; set; }

    [CliOption("--python-version")]
    public string? PythonVersion { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--scale-tier")]
    public string? ScaleTier { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--staging-bucket")]
    public string? StagingBucket { get; set; }

    [CliOption("--use-chief-in-tf-config")]
    public string? UseChiefInTfConfig { get; set; }

    [CliOption("--worker-accelerator")]
    public string[]? WorkerAccelerator { get; set; }

    [CliOption("--worker-image-uri")]
    public string? WorkerImageUri { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--stream-logs")]
    public bool? StreamLogs { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }

    [CliOption("--parameter-server-count")]
    public string? ParameterServerCount { get; set; }

    [CliOption("--parameter-server-machine-type")]
    public string? ParameterServerMachineType { get; set; }

    [CliOption("--worker-count")]
    public string? WorkerCount { get; set; }

    [CliOption("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }
}