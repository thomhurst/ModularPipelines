using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "remove-trust-store-revocations")]
public record AwsElbv2RemoveTrustStoreRevocationsOptions(
[property: CliOption("--trust-store-arn")] string TrustStoreArn,
[property: CliOption("--revocation-ids")] string[] RevocationIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}