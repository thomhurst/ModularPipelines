using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "create-rule")]
public record AwsFrauddetectorCreateRuleOptions(
[property: CommandSwitch("--rule-id")] string RuleId,
[property: CommandSwitch("--detector-id")] string DetectorId,
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