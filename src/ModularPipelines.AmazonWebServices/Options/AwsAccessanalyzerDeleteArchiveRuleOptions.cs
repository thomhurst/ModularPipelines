using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "delete-archive-rule")]
public record AwsAccessanalyzerDeleteArchiveRuleOptions(
[property: CommandSwitch("--analyzer-name")] string AnalyzerName,
[property: CommandSwitch("--rule-name")] string RuleName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}