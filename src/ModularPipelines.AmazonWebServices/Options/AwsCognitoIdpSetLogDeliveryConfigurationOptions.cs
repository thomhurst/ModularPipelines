using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "set-log-delivery-configuration")]
public record AwsCognitoIdpSetLogDeliveryConfigurationOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--log-configurations")] string[] LogConfigurations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}