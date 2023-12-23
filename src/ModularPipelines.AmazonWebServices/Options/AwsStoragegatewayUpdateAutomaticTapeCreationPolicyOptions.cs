using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-automatic-tape-creation-policy")]
public record AwsStoragegatewayUpdateAutomaticTapeCreationPolicyOptions(
[property: CommandSwitch("--automatic-tape-creation-rules")] string[] AutomaticTapeCreationRules,
[property: CommandSwitch("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}