using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "get-trust-store-revocation-content")]
public record AwsElbv2GetTrustStoreRevocationContentOptions(
[property: CliOption("--trust-store-arn")] string TrustStoreArn,
[property: CliOption("--revocation-id")] long RevocationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}