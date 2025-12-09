using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "register-user")]
public record AwsQuicksightRegisterUserOptions(
[property: CliOption("--identity-type")] string IdentityType,
[property: CliOption("--email")] string Email,
[property: CliOption("--user-role")] string UserRole,
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--iam-arn")]
    public string? IamArn { get; set; }

    [CliOption("--session-name")]
    public string? SessionName { get; set; }

    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--custom-permissions-name")]
    public string? CustomPermissionsName { get; set; }

    [CliOption("--external-login-federation-provider-type")]
    public string? ExternalLoginFederationProviderType { get; set; }

    [CliOption("--custom-federation-provider-url")]
    public string? CustomFederationProviderUrl { get; set; }

    [CliOption("--external-login-id")]
    public string? ExternalLoginId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}