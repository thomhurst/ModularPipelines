using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-user")]
public record AwsQuicksightUpdateUserOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--role")] string Role
) : AwsOptions
{
    [CommandSwitch("--custom-permissions-name")]
    public string? CustomPermissionsName { get; set; }

    [CommandSwitch("--external-login-federation-provider-type")]
    public string? ExternalLoginFederationProviderType { get; set; }

    [CommandSwitch("--custom-federation-provider-url")]
    public string? CustomFederationProviderUrl { get; set; }

    [CommandSwitch("--external-login-id")]
    public string? ExternalLoginId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}