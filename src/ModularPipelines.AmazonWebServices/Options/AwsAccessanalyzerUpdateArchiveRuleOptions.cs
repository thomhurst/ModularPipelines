using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "update-archive-rule")]
public record AwsAccessanalyzerUpdateArchiveRuleOptions(
[property: CommandSwitch("--analyzer-name")] string AnalyzerName,
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--filter")] IEnumerable<KeyValue> Filter
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}