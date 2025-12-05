using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "update-channel-class")]
public record AwsMedialiveUpdateChannelClassOptions(
[property: CliOption("--channel-class")] string ChannelClass,
[property: CliOption("--channel-id")] string ChannelId
) : AwsOptions
{
    [CliOption("--destinations")]
    public string[]? Destinations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}