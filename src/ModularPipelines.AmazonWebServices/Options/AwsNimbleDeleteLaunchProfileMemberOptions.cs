using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "delete-launch-profile-member")]
public record AwsNimbleDeleteLaunchProfileMemberOptions(
[property: CliOption("--launch-profile-id")] string LaunchProfileId,
[property: CliOption("--principal-id")] string PrincipalId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}