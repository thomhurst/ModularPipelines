using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "get-effective-policies")]
public record AwsIotGetEffectivePoliciesOptions : AwsOptions
{
    [CommandSwitch("--principal")]
    public string? Principal { get; set; }

    [CommandSwitch("--cognito-identity-pool-id")]
    public string? CognitoIdentityPoolId { get; set; }

    [CommandSwitch("--thing-name")]
    public string? ThingName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}