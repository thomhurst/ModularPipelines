using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-user")]
public record AwsQuicksightUpdateUserOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--email")] string Email,
[property: CliOption("--role")] string Role
) : AwsOptions
{
    [CliOption("--custom-permissions-name")]
    public string? CustomPermissionsName { get; set; }

    [CliOption("--external-login-federation-provider-type")]
    public string? ExternalLoginFederationProviderType { get; set; }

    [CliOption("--custom-federation-provider-url")]
    public string? CustomFederationProviderUrl { get; set; }

    [CliOption("--external-login-id")]
    public string? ExternalLoginId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}