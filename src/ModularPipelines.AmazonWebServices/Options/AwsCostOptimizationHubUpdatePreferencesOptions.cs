using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cost-optimization-hub", "update-preferences")]
public record AwsCostOptimizationHubUpdatePreferencesOptions : AwsOptions
{
    [CliOption("--member-account-discount-visibility")]
    public string? MemberAccountDiscountVisibility { get; set; }

    [CliOption("--savings-estimation-mode")]
    public string? SavingsEstimationMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}