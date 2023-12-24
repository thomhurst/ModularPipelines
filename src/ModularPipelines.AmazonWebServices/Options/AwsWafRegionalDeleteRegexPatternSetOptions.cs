using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "delete-regex-pattern-set")]
public record AwsWafRegionalDeleteRegexPatternSetOptions(
[property: CommandSwitch("--regex-pattern-set-id")] string RegexPatternSetId,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}