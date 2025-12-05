using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "create-rule")]
public record AwsFrauddetectorCreateRuleOptions(
[property: CliOption("--rule-id")] string RuleId,
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--expression")] string Expression,
[property: CliOption("--language")] string Language,
[property: CliOption("--outcomes")] string[] Outcomes
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}