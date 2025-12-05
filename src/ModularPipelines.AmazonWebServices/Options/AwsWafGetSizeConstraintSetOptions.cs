using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "get-size-constraint-set")]
public record AwsWafGetSizeConstraintSetOptions(
[property: CliOption("--size-constraint-set-id")] string SizeConstraintSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}