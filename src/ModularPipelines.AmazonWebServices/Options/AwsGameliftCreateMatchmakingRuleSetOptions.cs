using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-matchmaking-rule-set")]
public record AwsGameliftCreateMatchmakingRuleSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--rule-set-body")] string RuleSetBody
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}