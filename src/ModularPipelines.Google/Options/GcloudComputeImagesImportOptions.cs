using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "import")]
public record GcloudComputeImagesImportOptions(
[property: PositionalArgument] string ImageName,
[property: CommandSwitch("--source-file")] string SourceFile,
[property: CommandSwitch("--source-image")] string SourceImage,
[property: CommandSwitch("--aws-access-key-id")] string AwsAccessKeyId,
[property: CommandSwitch("--aws-region")] string AwsRegion,
[property: CommandSwitch("--aws-secret-access-key")] string AwsSecretAccessKey,
[property: CommandSwitch("--aws-session-token")] string AwsSessionToken,
[property: CommandSwitch("--aws-source-ami-file-path")] string AwsSourceAmiFilePath,
[property: CommandSwitch("--aws-ami-export-location")] string AwsAmiExportLocation,
[property: CommandSwitch("--aws-ami-id")] string AwsAmiId
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-address")]
    public bool? NoAddress { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cloudbuild-service-account")]
    public string? CloudbuildServiceAccount { get; set; }

    [CommandSwitch("--compute-service-account")]
    public string? ComputeServiceAccount { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--family")]
    public string? Family { get; set; }

    [BooleanCommandSwitch("--guest-environment")]
    public bool? GuestEnvironment { get; set; }

    [CommandSwitch("--guest-os-features")]
    public string[]? GuestOsFeatures { get; set; }

    [CommandSwitch("--log-location")]
    public string? LogLocation { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--storage-location")]
    public string? StorageLocation { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--data-disk")]
    public bool? DataDisk { get; set; }

    [BooleanCommandSwitch("--byol")]
    public bool? Byol { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }
}