using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "get-effective-policies")]
public record AwsIotGetEffectivePoliciesOptions : AwsOptions
{
    [CliOption("--principal")]
    public string? Principal { get; set; }

    [CliOption("--cognito-identity-pool-id")]
    public string? CognitoIdentityPoolId { get; set; }

    [CliOption("--thing-name")]
    public string? ThingName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}