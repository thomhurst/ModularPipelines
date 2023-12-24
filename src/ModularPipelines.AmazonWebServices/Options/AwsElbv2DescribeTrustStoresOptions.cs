using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "describe-trust-stores")]
public record AwsElbv2DescribeTrustStoresOptions : AwsOptions
{
    [CommandSwitch("--trust-store-arns")]
    public string[]? TrustStoreArns { get; set; }

    [CommandSwitch("--names")]
    public string[]? Names { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}