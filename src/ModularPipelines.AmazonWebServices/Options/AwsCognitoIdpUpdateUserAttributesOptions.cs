using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "update-user-attributes")]
public record AwsCognitoIdpUpdateUserAttributesOptions(
[property: CommandSwitch("--user-attributes")] string[] UserAttributes,
[property: CommandSwitch("--access-token")] string AccessToken
) : AwsOptions
{
    [CommandSwitch("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}