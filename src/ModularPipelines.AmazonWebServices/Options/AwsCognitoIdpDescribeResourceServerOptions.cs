using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "describe-resource-server")]
public record AwsCognitoIdpDescribeResourceServerOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}