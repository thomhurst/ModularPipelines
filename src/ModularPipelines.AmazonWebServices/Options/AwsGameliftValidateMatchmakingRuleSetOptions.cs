using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "validate-matchmaking-rule-set")]
public record AwsGameliftValidateMatchmakingRuleSetOptions(
[property: CommandSwitch("--rule-set-body")] string RuleSetBody
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}