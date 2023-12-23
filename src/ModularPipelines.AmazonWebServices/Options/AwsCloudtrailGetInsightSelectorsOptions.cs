using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "get-insight-selectors")]
public record AwsCloudtrailGetInsightSelectorsOptions : AwsOptions
{
    [CommandSwitch("--trail-name")]
    public string? TrailName { get; set; }

    [CommandSwitch("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}