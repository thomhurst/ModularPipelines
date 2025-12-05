using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "flex-template", "build")]
public record GcloudDataflowFlexTemplateBuildOptions(
[property: CliArgument] string TemplateFileGcsPath,
[property: CliOption("--sdk-language")] string SdkLanguage,
[property: CliOption("--image")] string Image,
[property: CliOption("--env")] string[] Env,
[property: CliOption("--flex-template-base-image")] string FlexTemplateBaseImage,
[property: CliOption("--image-gcr-path")] string ImageGcrPath,
[property: CliOption("--go-binary-path")] string GoBinaryPath,
[property: CliOption("--jar")] string[] Jar,
[property: CliOption("--py-path")] string[] PyPath
) : GcloudOptions
{
    [CliOption("--additional-experiments")]
    public string[]? AdditionalExperiments { get; set; }

    [CliOption("--additional-user-labels")]
    public string[]? AdditionalUserLabels { get; set; }

    [CliOption("--dataflow-kms-key")]
    public string? DataflowKmsKey { get; set; }

    [CliFlag("--disable-public-ips")]
    public bool? DisablePublicIps { get; set; }

    [CliFlag("--enable-streaming-engine")]
    public bool? EnableStreamingEngine { get; set; }

    [CliOption("--gcs-log-dir")]
    public string? GcsLogDir { get; set; }

    [CliOption("--image-repository-cert-path")]
    public string? ImageRepositoryCertPath { get; set; }

    [CliOption("--image-repository-password-secret-id")]
    public string? ImageRepositoryPasswordSecretId { get; set; }

    [CliOption("--image-repository-username-secret-id")]
    public string? ImageRepositoryUsernameSecretId { get; set; }

    [CliOption("--max-workers")]
    public string? MaxWorkers { get; set; }

    [CliOption("--metadata-file")]
    public string? MetadataFile { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--num-workers")]
    public string? NumWorkers { get; set; }

    [CliFlag("--print-only")]
    public bool? PrintOnly { get; set; }

    [CliOption("--service-account-email")]
    public string? ServiceAccountEmail { get; set; }

    [CliOption("--staging-location")]
    public string? StagingLocation { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliOption("--temp-location")]
    public string? TempLocation { get; set; }

    [CliOption("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CliOption("--worker-region")]
    public string? WorkerRegion { get; set; }

    [CliOption("--worker-zone")]
    public string? WorkerZone { get; set; }
}