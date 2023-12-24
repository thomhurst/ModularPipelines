using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "update-xss-match-set")]
public record AwsWafRegionalUpdateXssMatchSetOptions(
[property: CommandSwitch("--xss-match-set-id")] string XssMatchSetId,
[property: CommandSwitch("--change-token")] string ChangeToken,
[property: CommandSwitch("--updates")] string[] Updates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}