using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "remove-trust-store-revocations")]
public record AwsElbv2RemoveTrustStoreRevocationsOptions(
[property: CommandSwitch("--trust-store-arn")] string TrustStoreArn,
[property: CommandSwitch("--revocation-ids")] string[] RevocationIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}