using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "put-launch-profile-members")]
public record AwsNimblePutLaunchProfileMembersOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--launch-profile-id")] string LaunchProfileId,
[property: CliOption("--members")] string[] Members,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}