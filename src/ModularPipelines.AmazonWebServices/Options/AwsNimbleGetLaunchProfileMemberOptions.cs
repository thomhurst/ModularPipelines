using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "get-launch-profile-member")]
public record AwsNimbleGetLaunchProfileMemberOptions(
[property: CliOption("--launch-profile-id")] string LaunchProfileId,
[property: CliOption("--principal-id")] string PrincipalId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}