using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "get-regex-pattern-set")]
public record AwsWafv2GetRegexPatternSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}