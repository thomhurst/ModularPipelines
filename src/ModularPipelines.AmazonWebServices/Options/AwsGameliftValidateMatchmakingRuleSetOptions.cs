using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "validate-matchmaking-rule-set")]
public record AwsGameliftValidateMatchmakingRuleSetOptions(
[property: CliOption("--rule-set-body")] string RuleSetBody
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}