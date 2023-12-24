using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "associate-identity-provider-config")]
public record AwsEksAssociateIdentityProviderConfigOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--oidc")] string Oidc
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}