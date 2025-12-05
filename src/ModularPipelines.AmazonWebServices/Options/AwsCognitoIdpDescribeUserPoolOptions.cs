using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "describe-user-pool")]
public record AwsCognitoIdpDescribeUserPoolOptions(
[property: CliOption("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}