using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "update-log-pattern")]
public record AwsApplicationInsightsUpdateLogPatternOptions(
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName,
[property: CommandSwitch("--pattern-set-name")] string PatternSetName,
[property: CommandSwitch("--pattern-name")] string PatternName,
[property: CommandSwitch("--pattern")] string Pattern
) : AwsOptions
{
    [CommandSwitch("--rank")]
    public int? Rank { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}