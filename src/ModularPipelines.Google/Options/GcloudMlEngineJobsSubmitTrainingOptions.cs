using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "jobs", "submit", "training")]
public record GcloudMlEngineJobsSubmitTrainingOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string UserArgs
) : GcloudOptions
{
    [CommandSwitch("--config")]
    public string? Config { get; set; }

    [BooleanCommandSwitch("--enable-web-access")]
    public bool? EnableWebAccess { get; set; }

    [CommandSwitch("--job-dir")]
    public string? JobDir { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--master-accelerator")]
    public string[]? MasterAccelerator { get; set; }

    [CommandSwitch("--master-image-uri")]
    public string? MasterImageUri { get; set; }

    [CommandSwitch("--master-machine-type")]
    public string? MasterMachineType { get; set; }

    [CommandSwitch("--module-name")]
    public string? ModuleName { get; set; }

    [CommandSwitch("--package-path")]
    public string? PackagePath { get; set; }

    [CommandSwitch("--packages")]
    public string[]? Packages { get; set; }

    [CommandSwitch("--parameter-server-accelerator")]
    public string[]? ParameterServerAccelerator { get; set; }

    [CommandSwitch("--parameter-server-image-uri")]
    public string? ParameterServerImageUri { get; set; }

    [CommandSwitch("--python-version")]
    public string? PythonVersion { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--scale-tier")]
    public string? ScaleTier { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--staging-bucket")]
    public string? StagingBucket { get; set; }

    [CommandSwitch("--use-chief-in-tf-config")]
    public string? UseChiefInTfConfig { get; set; }

    [CommandSwitch("--worker-accelerator")]
    public string[]? WorkerAccelerator { get; set; }

    [CommandSwitch("--worker-image-uri")]
    public string? WorkerImageUri { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--stream-logs")]
    public bool? StreamLogs { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }

    [CommandSwitch("--parameter-server-count")]
    public string? ParameterServerCount { get; set; }

    [CommandSwitch("--parameter-server-machine-type")]
    public string? ParameterServerMachineType { get; set; }

    [CommandSwitch("--worker-count")]
    public string? WorkerCount { get; set; }

    [CommandSwitch("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }
}