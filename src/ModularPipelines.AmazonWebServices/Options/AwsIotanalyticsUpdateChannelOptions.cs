using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "update-channel")]
public record AwsIotanalyticsUpdateChannelOptions(
[property: CommandSwitch("--channel-name")] string ChannelName
) : AwsOptions
{
    [CommandSwitch("--channel-storage")]
    public string? ChannelStorage { get; set; }

    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}