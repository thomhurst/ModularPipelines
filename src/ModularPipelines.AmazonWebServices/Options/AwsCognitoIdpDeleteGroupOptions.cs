using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "delete-group")]
public record AwsCognitoIdpDeleteGroupOptions(
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}