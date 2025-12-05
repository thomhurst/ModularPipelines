using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-automatic-tape-creation-policy")]
public record AwsStoragegatewayUpdateAutomaticTapeCreationPolicyOptions(
[property: CliOption("--automatic-tape-creation-rules")] string[] AutomaticTapeCreationRules,
[property: CliOption("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}