using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "disassociate-identity-provider-config")]
public record AwsEksDisassociateIdentityProviderConfigOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--identity-provider-config")] string IdentityProviderConfig
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}