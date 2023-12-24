using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "create-user-pool-domain")]
public record AwsCognitoIdpCreateUserPoolDomainOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CommandSwitch("--custom-domain-config")]
    public string? CustomDomainConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}