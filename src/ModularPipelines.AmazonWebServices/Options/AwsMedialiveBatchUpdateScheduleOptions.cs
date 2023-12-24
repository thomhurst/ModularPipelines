using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "batch-update-schedule")]
public record AwsMedialiveBatchUpdateScheduleOptions(
[property: CommandSwitch("--channel-id")] string ChannelId
) : AwsOptions
{
    [CommandSwitch("--creates")]
    public string? Creates { get; set; }

    [CommandSwitch("--deletes")]
    public string? Deletes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}