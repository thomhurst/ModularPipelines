using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cost-optimization-hub", "update-preferences")]
public record AwsCostOptimizationHubUpdatePreferencesOptions : AwsOptions
{
    [CommandSwitch("--member-account-discount-visibility")]
    public string? MemberAccountDiscountVisibility { get; set; }

    [CommandSwitch("--savings-estimation-mode")]
    public string? SavingsEstimationMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}