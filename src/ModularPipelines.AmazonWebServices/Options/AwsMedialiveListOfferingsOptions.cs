using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "list-offerings")]
public record AwsMedialiveListOfferingsOptions : AwsOptions
{
    [CliOption("--channel-class")]
    public string? ChannelClass { get; set; }

    [CliOption("--channel-configuration")]
    public string? ChannelConfiguration { get; set; }

    [CliOption("--codec")]
    public string? Codec { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--maximum-bitrate")]
    public string? MaximumBitrate { get; set; }

    [CliOption("--maximum-framerate")]
    public string? MaximumFramerate { get; set; }

    [CliOption("--resolution")]
    public string? Resolution { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--special-feature")]
    public string? SpecialFeature { get; set; }

    [CliOption("--video-quality")]
    public string? VideoQuality { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}