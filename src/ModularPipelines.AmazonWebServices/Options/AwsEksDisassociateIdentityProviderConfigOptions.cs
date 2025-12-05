using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "disassociate-identity-provider-config")]
public record AwsEksDisassociateIdentityProviderConfigOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--identity-provider-config")] string IdentityProviderConfig
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}