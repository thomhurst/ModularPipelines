using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "register-user")]
public record AwsQuicksightRegisterUserOptions(
[property: CommandSwitch("--identity-type")] string IdentityType,
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--user-role")] string UserRole,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
    [CommandSwitch("--iam-arn")]
    public string? IamArn { get; set; }

    [CommandSwitch("--session-name")]
    public string? SessionName { get; set; }

    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--custom-permissions-name")]
    public string? CustomPermissionsName { get; set; }

    [CommandSwitch("--external-login-federation-provider-type")]
    public string? ExternalLoginFederationProviderType { get; set; }

    [CommandSwitch("--custom-federation-provider-url")]
    public string? CustomFederationProviderUrl { get; set; }

    [CommandSwitch("--external-login-id")]
    public string? ExternalLoginId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}