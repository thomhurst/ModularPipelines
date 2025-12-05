using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "add-notification-channels")]
public record AwsCodeguruprofilerAddNotificationChannelsOptions(
[property: CliOption("--channels")] string[] Channels,
[property: CliOption("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}