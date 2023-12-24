using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "get-size-constraint-set")]
public record AwsWafRegionalGetSizeConstraintSetOptions(
[property: CommandSwitch("--size-constraint-set-id")] string SizeConstraintSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}