using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "update-pricing-plan")]
public record AwsIottwinmakerUpdatePricingPlanOptions(
[property: CommandSwitch("--pricing-mode")] string PricingMode
) : AwsOptions
{
    [CommandSwitch("--bundle-names")]
    public string[]? BundleNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}