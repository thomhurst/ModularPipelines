using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "delete-trust-store")]
public record AwsElbv2DeleteTrustStoreOptions(
[property: CliOption("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}