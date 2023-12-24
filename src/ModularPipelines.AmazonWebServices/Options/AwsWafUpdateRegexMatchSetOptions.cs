using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf", "update-regex-match-set")]
public record AwsWafUpdateRegexMatchSetOptions(
[property: CommandSwitch("--regex-match-set-id")] string RegexMatchSetId,
[property: CommandSwitch("--updates")] string[] Updates,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}