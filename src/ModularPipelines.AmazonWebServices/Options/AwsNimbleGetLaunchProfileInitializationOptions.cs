using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "get-launch-profile-initialization")]
public record AwsNimbleGetLaunchProfileInitializationOptions(
[property: CommandSwitch("--launch-profile-id")] string LaunchProfileId,
[property: CommandSwitch("--launch-profile-protocol-versions")] string[] LaunchProfileProtocolVersions,
[property: CommandSwitch("--launch-purpose")] string LaunchPurpose,
[property: CommandSwitch("--platform")] string Platform,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}