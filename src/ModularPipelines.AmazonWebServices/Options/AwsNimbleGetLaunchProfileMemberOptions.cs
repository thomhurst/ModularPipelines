using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "get-launch-profile-member")]
public record AwsNimbleGetLaunchProfileMemberOptions(
[property: CommandSwitch("--launch-profile-id")] string LaunchProfileId,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}