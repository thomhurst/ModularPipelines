using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "create-identity-provider")]
public record AwsWorkspacesWebCreateIdentityProviderOptions(
[property: CliOption("--identity-provider-details")] IEnumerable<KeyValue> IdentityProviderDetails,
[property: CliOption("--identity-provider-name")] string IdentityProviderName,
[property: CliOption("--identity-provider-type")] string IdentityProviderType,
[property: CliOption("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}