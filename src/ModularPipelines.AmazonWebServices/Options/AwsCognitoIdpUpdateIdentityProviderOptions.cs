using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "update-identity-provider")]
public record AwsCognitoIdpUpdateIdentityProviderOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--provider-name")] string ProviderName
) : AwsOptions
{
    [CommandSwitch("--provider-details")]
    public IEnumerable<KeyValue>? ProviderDetails { get; set; }

    [CommandSwitch("--attribute-mapping")]
    public IEnumerable<KeyValue>? AttributeMapping { get; set; }

    [CommandSwitch("--idp-identifiers")]
    public string[]? IdpIdentifiers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}