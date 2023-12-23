using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-matchmaking-rule-set")]
public record AwsGameliftCreateMatchmakingRuleSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--rule-set-body")] string RuleSetBody
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}