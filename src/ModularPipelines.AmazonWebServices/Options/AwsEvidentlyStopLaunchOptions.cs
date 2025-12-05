using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "stop-launch")]
public record AwsEvidentlyStopLaunchOptions(
[property: CliOption("--launch")] string Launch,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--desired-state")]
    public string? DesiredState { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}