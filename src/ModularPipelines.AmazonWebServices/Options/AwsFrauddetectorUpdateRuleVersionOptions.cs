using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "update-rule-version")]
public record AwsFrauddetectorUpdateRuleVersionOptions(
[property: CommandSwitch("--rule")] string Rule,
[property: CommandSwitch("--expression")] string Expression,
[property: CommandSwitch("--language")] string Language,
[property: CommandSwitch("--outcomes")] string[] Outcomes
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}