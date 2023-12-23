using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "list-offerings")]
public record AwsMedialiveListOfferingsOptions : AwsOptions
{
    [CommandSwitch("--channel-class")]
    public string? ChannelClass { get; set; }

    [CommandSwitch("--channel-configuration")]
    public string? ChannelConfiguration { get; set; }

    [CommandSwitch("--codec")]
    public string? Codec { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--maximum-bitrate")]
    public string? MaximumBitrate { get; set; }

    [CommandSwitch("--maximum-framerate")]
    public string? MaximumFramerate { get; set; }

    [CommandSwitch("--resolution")]
    public string? Resolution { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--special-feature")]
    public string? SpecialFeature { get; set; }

    [CommandSwitch("--video-quality")]
    public string? VideoQuality { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}