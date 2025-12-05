using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-user-profile")]
public record AwsSagemakerDeleteUserProfileOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}