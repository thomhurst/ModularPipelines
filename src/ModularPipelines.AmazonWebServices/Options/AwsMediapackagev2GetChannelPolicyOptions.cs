using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackagev2", "get-channel-policy")]
public record AwsMediapackagev2GetChannelPolicyOptions(
[property: CliOption("--channel-group-name")] string ChannelGroupName,
[property: CliOption("--channel-name")] string ChannelName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}