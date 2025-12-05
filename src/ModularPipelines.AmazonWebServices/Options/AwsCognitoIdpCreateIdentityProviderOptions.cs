using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "create-identity-provider")]
public record AwsCognitoIdpCreateIdentityProviderOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--provider-name")] string ProviderName,
[property: CliOption("--provider-type")] string ProviderType,
[property: CliOption("--provider-details")] IEnumerable<KeyValue> ProviderDetails
) : AwsOptions
{
    [CliOption("--attribute-mapping")]
    public IEnumerable<KeyValue>? AttributeMapping { get; set; }

    [CliOption("--idp-identifiers")]
    public string[]? IdpIdentifiers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}