using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "check-capacity")]
public record AwsWafv2CheckCapacityOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--rules")] string[] Rules
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}