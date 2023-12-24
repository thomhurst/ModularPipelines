using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "get-log-delivery-configuration")]
public record AwsCognitoIdpGetLogDeliveryConfigurationOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}