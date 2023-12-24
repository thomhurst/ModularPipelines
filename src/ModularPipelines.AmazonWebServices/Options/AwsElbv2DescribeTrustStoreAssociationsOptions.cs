using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "describe-trust-store-associations")]
public record AwsElbv2DescribeTrustStoreAssociationsOptions(
[property: CommandSwitch("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}