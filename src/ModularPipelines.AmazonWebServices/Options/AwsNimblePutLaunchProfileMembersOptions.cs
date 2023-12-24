using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "put-launch-profile-members")]
public record AwsNimblePutLaunchProfileMembersOptions(
[property: CommandSwitch("--identity-store-id")] string IdentityStoreId,
[property: CommandSwitch("--launch-profile-id")] string LaunchProfileId,
[property: CommandSwitch("--members")] string[] Members,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}