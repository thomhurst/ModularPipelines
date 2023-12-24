using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "delete-user-pool-client")]
public record AwsCognitoIdpDeleteUserPoolClientOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--client-id")] string ClientId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}