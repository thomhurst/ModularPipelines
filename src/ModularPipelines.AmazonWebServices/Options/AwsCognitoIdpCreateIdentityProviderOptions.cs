using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "create-identity-provider")]
public record AwsCognitoIdpCreateIdentityProviderOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--provider-name")] string ProviderName,
[property: CommandSwitch("--provider-type")] string ProviderType,
[property: CommandSwitch("--provider-details")] IEnumerable<KeyValue> ProviderDetails
) : AwsOptions
{
    [CommandSwitch("--attribute-mapping")]
    public IEnumerable<KeyValue>? AttributeMapping { get; set; }

    [CommandSwitch("--idp-identifiers")]
    public string[]? IdpIdentifiers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}