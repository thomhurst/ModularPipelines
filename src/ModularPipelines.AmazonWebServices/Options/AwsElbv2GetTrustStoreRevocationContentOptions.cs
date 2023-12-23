using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "get-trust-store-revocation-content")]
public record AwsElbv2GetTrustStoreRevocationContentOptions(
[property: CommandSwitch("--trust-store-arn")] string TrustStoreArn,
[property: CommandSwitch("--revocation-id")] long RevocationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}