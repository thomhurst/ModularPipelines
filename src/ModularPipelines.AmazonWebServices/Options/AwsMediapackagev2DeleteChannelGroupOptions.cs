using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackagev2", "delete-channel-group")]
public record AwsMediapackagev2DeleteChannelGroupOptions(
[property: CliOption("--channel-group-name")] string ChannelGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}