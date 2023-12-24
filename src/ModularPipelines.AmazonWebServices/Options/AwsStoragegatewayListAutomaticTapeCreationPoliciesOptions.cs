using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "list-automatic-tape-creation-policies")]
public record AwsStoragegatewayListAutomaticTapeCreationPoliciesOptions : AwsOptions
{
    [CommandSwitch("--gateway-arn")]
    public string? GatewayArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}