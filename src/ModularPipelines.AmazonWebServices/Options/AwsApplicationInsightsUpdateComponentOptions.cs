using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "update-component")]
public record AwsApplicationInsightsUpdateComponentOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--component-name")] string ComponentName
) : AwsOptions
{
    [CliOption("--new-component-name")]
    public string? NewComponentName { get; set; }

    [CliOption("--resource-list")]
    public string[]? ResourceList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}