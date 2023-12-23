using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "revoke-token")]
public record AwsCognitoIdpRevokeTokenOptions(
[property: CommandSwitch("--token")] string Token,
[property: CommandSwitch("--client-id")] string ClientId
) : AwsOptions
{
    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}