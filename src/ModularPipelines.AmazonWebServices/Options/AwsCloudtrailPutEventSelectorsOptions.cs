using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "put-event-selectors")]
public record AwsCloudtrailPutEventSelectorsOptions(
[property: CommandSwitch("--trail-name")] string TrailName
) : AwsOptions
{
    [CommandSwitch("--event-selectors")]
    public string[]? EventSelectors { get; set; }

    [CommandSwitch("--advanced-event-selectors")]
    public string[]? AdvancedEventSelectors { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}