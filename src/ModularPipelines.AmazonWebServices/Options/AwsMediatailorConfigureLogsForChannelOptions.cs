using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "configure-logs-for-channel")]
public record AwsMediatailorConfigureLogsForChannelOptions(
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--log-types")] string[] LogTypes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}