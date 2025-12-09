using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager-user-subscriptions", "associate-user")]
public record AwsLicenseManagerUserSubscriptionsAssociateUserOptions(
[property: CliOption("--identity-provider")] string IdentityProvider,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--username")] string Username
) : AwsOptions
{
    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}