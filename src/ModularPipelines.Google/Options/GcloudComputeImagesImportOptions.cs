using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "import")]
public record GcloudComputeImagesImportOptions(
[property: CliArgument] string ImageName,
[property: CliOption("--source-file")] string SourceFile,
[property: CliOption("--source-image")] string SourceImage,
[property: CliOption("--aws-access-key-id")] string AwsAccessKeyId,
[property: CliOption("--aws-region")] string AwsRegion,
[property: CliOption("--aws-secret-access-key")] string AwsSecretAccessKey,
[property: CliOption("--aws-session-token")] string AwsSessionToken,
[property: CliOption("--aws-source-ami-file-path")] string AwsSourceAmiFilePath,
[property: CliOption("--aws-ami-export-location")] string AwsAmiExportLocation,
[property: CliOption("--aws-ami-id")] string AwsAmiId
) : GcloudOptions
{
    [CliFlag("--no-address")]
    public bool? NoAddress { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cloudbuild-service-account")]
    public string? CloudbuildServiceAccount { get; set; }

    [CliOption("--compute-service-account")]
    public string? ComputeServiceAccount { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliFlag("--guest-environment")]
    public bool? GuestEnvironment { get; set; }

    [CliOption("--guest-os-features")]
    public string[]? GuestOsFeatures { get; set; }

    [CliOption("--log-location")]
    public string? LogLocation { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--data-disk")]
    public bool? DataDisk { get; set; }

    [CliFlag("--byol")]
    public bool? Byol { get; set; }

    [CliOption("--os")]
    public string? Os { get; set; }
}