using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "get-notification-configuration")]
public record AwsCodeguruprofilerGetNotificationConfigurationOptions(
[property: CliOption("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}