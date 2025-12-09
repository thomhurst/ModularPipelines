using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "update-component-configuration")]
public record AwsApplicationInsightsUpdateComponentConfigurationOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--component-name")] string ComponentName
) : AwsOptions
{
    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--component-configuration")]
    public string? ComponentConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}