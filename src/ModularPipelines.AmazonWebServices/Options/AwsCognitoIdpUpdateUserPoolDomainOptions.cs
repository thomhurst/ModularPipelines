using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "update-user-pool-domain")]
public record AwsCognitoIdpUpdateUserPoolDomainOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--custom-domain-config")] string CustomDomainConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}