using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "flex-template", "build")]
public record GcloudDataflowFlexTemplateBuildOptions(
[property: PositionalArgument] string TemplateFileGcsPath,
[property: CommandSwitch("--sdk-language")] string SdkLanguage,
[property: CommandSwitch("--image")] string Image,
[property: CommandSwitch("--env")] string[] Env,
[property: CommandSwitch("--flex-template-base-image")] string FlexTemplateBaseImage,
[property: CommandSwitch("--image-gcr-path")] string ImageGcrPath,
[property: CommandSwitch("--go-binary-path")] string GoBinaryPath,
[property: CommandSwitch("--jar")] string[] Jar,
[property: CommandSwitch("--py-path")] string[] PyPath
) : GcloudOptions
{
    [CommandSwitch("--additional-experiments")]
    public string[]? AdditionalExperiments { get; set; }

    [CommandSwitch("--additional-user-labels")]
    public string[]? AdditionalUserLabels { get; set; }

    [CommandSwitch("--dataflow-kms-key")]
    public string? DataflowKmsKey { get; set; }

    [BooleanCommandSwitch("--disable-public-ips")]
    public bool? DisablePublicIps { get; set; }

    [BooleanCommandSwitch("--enable-streaming-engine")]
    public bool? EnableStreamingEngine { get; set; }

    [CommandSwitch("--gcs-log-dir")]
    public string? GcsLogDir { get; set; }

    [CommandSwitch("--image-repository-cert-path")]
    public string? ImageRepositoryCertPath { get; set; }

    [CommandSwitch("--image-repository-password-secret-id")]
    public string? ImageRepositoryPasswordSecretId { get; set; }

    [CommandSwitch("--image-repository-username-secret-id")]
    public string? ImageRepositoryUsernameSecretId { get; set; }

    [CommandSwitch("--max-workers")]
    public string? MaxWorkers { get; set; }

    [CommandSwitch("--metadata-file")]
    public string? MetadataFile { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--num-workers")]
    public string? NumWorkers { get; set; }

    [BooleanCommandSwitch("--print-only")]
    public bool? PrintOnly { get; set; }

    [CommandSwitch("--service-account-email")]
    public string? ServiceAccountEmail { get; set; }

    [CommandSwitch("--staging-location")]
    public string? StagingLocation { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CommandSwitch("--temp-location")]
    public string? TempLocation { get; set; }

    [CommandSwitch("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CommandSwitch("--worker-region")]
    public string? WorkerRegion { get; set; }

    [CommandSwitch("--worker-zone")]
    public string? WorkerZone { get; set; }
}