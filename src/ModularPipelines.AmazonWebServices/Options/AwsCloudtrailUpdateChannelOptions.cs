using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "update-channel")]
public record AwsCloudtrailUpdateChannelOptions(
[property: CommandSwitch("--channel")] string Channel
) : AwsOptions
{
    [CommandSwitch("--destinations")]
    public string[]? Destinations { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}