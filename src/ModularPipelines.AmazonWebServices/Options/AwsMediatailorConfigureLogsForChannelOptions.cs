using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "configure-logs-for-channel")]
public record AwsMediatailorConfigureLogsForChannelOptions(
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--log-types")] string[] LogTypes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}