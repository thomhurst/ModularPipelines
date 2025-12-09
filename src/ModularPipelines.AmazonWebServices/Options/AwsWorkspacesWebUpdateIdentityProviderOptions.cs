using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "update-identity-provider")]
public record AwsWorkspacesWebUpdateIdentityProviderOptions(
[property: CliOption("--identity-provider-arn")] string IdentityProviderArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--identity-provider-details")]
    public IEnumerable<KeyValue>? IdentityProviderDetails { get; set; }

    [CliOption("--identity-provider-name")]
    public string? IdentityProviderName { get; set; }

    [CliOption("--identity-provider-type")]
    public string? IdentityProviderType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}