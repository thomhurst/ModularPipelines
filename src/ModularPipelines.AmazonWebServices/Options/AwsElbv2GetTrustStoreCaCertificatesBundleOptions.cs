using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "get-trust-store-ca-certificates-bundle")]
public record AwsElbv2GetTrustStoreCaCertificatesBundleOptions(
[property: CommandSwitch("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}