using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "get-rules")]
public record AwsFrauddetectorGetRulesOptions(
[property: CommandSwitch("--detector-id")] string DetectorId
) : AwsOptions
{
    [CommandSwitch("--rule-id")]
    public string? RuleId { get; set; }

    [CommandSwitch("--rule-version")]
    public string? RuleVersion { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}