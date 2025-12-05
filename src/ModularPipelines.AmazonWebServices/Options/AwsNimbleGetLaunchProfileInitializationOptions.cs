using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "get-launch-profile-initialization")]
public record AwsNimbleGetLaunchProfileInitializationOptions(
[property: CliOption("--launch-profile-id")] string LaunchProfileId,
[property: CliOption("--launch-profile-protocol-versions")] string[] LaunchProfileProtocolVersions,
[property: CliOption("--launch-purpose")] string LaunchPurpose,
[property: CliOption("--platform")] string Platform,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}