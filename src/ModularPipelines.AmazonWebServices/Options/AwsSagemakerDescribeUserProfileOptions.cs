using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-user-profile")]
public record AwsSagemakerDescribeUserProfileOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}