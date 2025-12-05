using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "describe-identity-provider")]
public record AwsCognitoIdpDescribeIdentityProviderOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--provider-name")] string ProviderName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}