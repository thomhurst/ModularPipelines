using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "update-regex-pattern-set")]
public record AwsWafRegionalUpdateRegexPatternSetOptions(
[property: CommandSwitch("--regex-pattern-set-id")] string RegexPatternSetId,
[property: CommandSwitch("--updates")] string[] Updates,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}