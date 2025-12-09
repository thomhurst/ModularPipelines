using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "update-identity-provider")]
public record AwsCognitoIdpUpdateIdentityProviderOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--provider-name")] string ProviderName
) : AwsOptions
{
    [CliOption("--provider-details")]
    public IEnumerable<KeyValue>? ProviderDetails { get; set; }

    [CliOption("--attribute-mapping")]
    public IEnumerable<KeyValue>? AttributeMapping { get; set; }

    [CliOption("--idp-identifiers")]
    public string[]? IdpIdentifiers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}