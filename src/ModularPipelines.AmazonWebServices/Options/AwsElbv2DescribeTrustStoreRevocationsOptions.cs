using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "describe-trust-store-revocations")]
public record AwsElbv2DescribeTrustStoreRevocationsOptions(
[property: CommandSwitch("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CommandSwitch("--revocation-ids")]
    public string[]? RevocationIds { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}