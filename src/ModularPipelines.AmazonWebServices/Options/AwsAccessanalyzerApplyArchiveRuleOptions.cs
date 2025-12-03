using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "apply-archive-rule")]
public record AwsAccessanalyzerApplyArchiveRuleOptions(
[property: CliOption("--analyzer-arn")] string AnalyzerArn,
[property: CliOption("--rule-name")] string RuleName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}