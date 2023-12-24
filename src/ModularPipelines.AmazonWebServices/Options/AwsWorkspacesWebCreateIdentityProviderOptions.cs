using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "create-identity-provider")]
public record AwsWorkspacesWebCreateIdentityProviderOptions(
[property: CommandSwitch("--identity-provider-details")] IEnumerable<KeyValue> IdentityProviderDetails,
[property: CommandSwitch("--identity-provider-name")] string IdentityProviderName,
[property: CommandSwitch("--identity-provider-type")] string IdentityProviderType,
[property: CommandSwitch("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}