using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "upload-build")]
public record AwsGameliftUploadBuildOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--build-version")] string BuildVersion,
[property: CommandSwitch("--build-root")] string BuildRoot
) : AwsOptions
{
    [CommandSwitch("--server-sdk-version")]
    public string? ServerSdkVersion { get; set; }

    [CommandSwitch("--operating-system")]
    public string? OperatingSystem { get; set; }
}