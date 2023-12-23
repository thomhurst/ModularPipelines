using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf", "get-sql-injection-match-set")]
public record AwsWafGetSqlInjectionMatchSetOptions(
[property: CommandSwitch("--sql-injection-match-set-id")] string SqlInjectionMatchSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}