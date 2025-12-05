using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "upload-build")]
public record AwsGameliftUploadBuildOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--build-version")] string BuildVersion,
[property: CliOption("--build-root")] string BuildRoot
) : AwsOptions
{
    [CliOption("--server-sdk-version")]
    public string? ServerSdkVersion { get; set; }

    [CliOption("--operating-system")]
    public string? OperatingSystem { get; set; }
}