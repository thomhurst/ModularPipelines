using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-user-profile")]
public record AwsSagemakerCreateUserProfileOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CliOption("--single-sign-on-user-identifier")]
    public string? SingleSignOnUserIdentifier { get; set; }

    [CliOption("--single-sign-on-user-value")]
    public string? SingleSignOnUserValue { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--user-settings")]
    public string? UserSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}