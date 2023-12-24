using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "update-launch-profile-member")]
public record AwsNimbleUpdateLaunchProfileMemberOptions(
[property: CommandSwitch("--launch-profile-id")] string LaunchProfileId,
[property: CommandSwitch("--persona")] string Persona,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}