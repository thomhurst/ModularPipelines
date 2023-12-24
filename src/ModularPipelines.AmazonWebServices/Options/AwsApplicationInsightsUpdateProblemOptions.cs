using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "update-problem")]
public record AwsApplicationInsightsUpdateProblemOptions(
[property: CommandSwitch("--problem-id")] string ProblemId
) : AwsOptions
{
    [CommandSwitch("--update-status")]
    public string? UpdateStatus { get; set; }

    [CommandSwitch("--visibility")]
    public string? Visibility { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}