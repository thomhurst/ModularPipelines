using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "delete-archive-rule")]
public record AwsAccessanalyzerDeleteArchiveRuleOptions(
[property: CliOption("--analyzer-name")] string AnalyzerName,
[property: CliOption("--rule-name")] string RuleName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}