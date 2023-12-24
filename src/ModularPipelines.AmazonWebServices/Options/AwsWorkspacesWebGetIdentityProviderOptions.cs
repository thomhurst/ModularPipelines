using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "get-identity-provider")]
public record AwsWorkspacesWebGetIdentityProviderOptions(
[property: CommandSwitch("--identity-provider-arn")] string IdentityProviderArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}