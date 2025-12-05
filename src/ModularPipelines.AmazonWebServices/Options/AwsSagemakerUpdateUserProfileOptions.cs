using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-user-profile")]
public record AwsSagemakerUpdateUserProfileOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CliOption("--user-settings")]
    public string? UserSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}