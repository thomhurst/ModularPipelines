using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "delete-identities")]
public record AwsCognitoIdentityDeleteIdentitiesOptions(
[property: CommandSwitch("--identity-ids-to-delete")] string[] IdentityIdsToDelete
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}