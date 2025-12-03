using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "update-user-pool-domain")]
public record AwsCognitoIdpUpdateUserPoolDomainOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--custom-domain-config")] string CustomDomainConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}