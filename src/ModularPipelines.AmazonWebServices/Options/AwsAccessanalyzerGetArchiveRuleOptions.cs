using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "get-archive-rule")]
public record AwsAccessanalyzerGetArchiveRuleOptions(
[property: CliOption("--analyzer-name")] string AnalyzerName,
[property: CliOption("--rule-name")] string RuleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}