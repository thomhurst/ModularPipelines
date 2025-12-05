using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "update-channel")]
public record AwsCloudtrailUpdateChannelOptions(
[property: CliOption("--channel")] string Channel
) : AwsOptions
{
    [CliOption("--destinations")]
    public string[]? Destinations { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}