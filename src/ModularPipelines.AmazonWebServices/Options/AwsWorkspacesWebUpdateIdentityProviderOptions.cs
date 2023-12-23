using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "update-identity-provider")]
public record AwsWorkspacesWebUpdateIdentityProviderOptions(
[property: CommandSwitch("--identity-provider-arn")] string IdentityProviderArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--identity-provider-details")]
    public IEnumerable<KeyValue>? IdentityProviderDetails { get; set; }

    [CommandSwitch("--identity-provider-name")]
    public string? IdentityProviderName { get; set; }

    [CommandSwitch("--identity-provider-type")]
    public string? IdentityProviderType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}