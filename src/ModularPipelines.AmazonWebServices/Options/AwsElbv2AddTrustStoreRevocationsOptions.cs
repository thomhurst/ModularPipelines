using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "add-trust-store-revocations")]
public record AwsElbv2AddTrustStoreRevocationsOptions(
[property: CliOption("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CliOption("--revocation-contents")]
    public string[]? RevocationContents { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}