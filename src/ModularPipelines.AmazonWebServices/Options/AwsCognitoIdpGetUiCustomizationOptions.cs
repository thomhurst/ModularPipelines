using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "get-ui-customization")]
public record AwsCognitoIdpGetUiCustomizationOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}