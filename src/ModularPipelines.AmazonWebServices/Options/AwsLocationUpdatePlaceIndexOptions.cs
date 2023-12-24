using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "update-place-index")]
public record AwsLocationUpdatePlaceIndexOptions(
[property: CommandSwitch("--index-name")] string IndexName
) : AwsOptions
{
    [CommandSwitch("--data-source-configuration")]
    public string? DataSourceConfiguration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}