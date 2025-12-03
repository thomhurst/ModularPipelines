using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "update-archive-rule")]
public record AwsAccessanalyzerUpdateArchiveRuleOptions(
[property: CliOption("--analyzer-name")] string AnalyzerName,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--filter")] IEnumerable<KeyValue> Filter
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}