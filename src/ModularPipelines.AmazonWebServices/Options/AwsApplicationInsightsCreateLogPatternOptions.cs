using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "create-log-pattern")]
public record AwsApplicationInsightsCreateLogPatternOptions(
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName,
[property: CommandSwitch("--pattern-set-name")] string PatternSetName,
[property: CommandSwitch("--pattern-name")] string PatternName,
[property: CommandSwitch("--pattern")] string Pattern,
[property: CommandSwitch("--rank")] int Rank
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}