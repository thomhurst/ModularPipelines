using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "set-rule-priorities")]
public record AwsElbv2SetRulePrioritiesOptions(
[property: CliOption("--rule-priorities")] string[] RulePriorities
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}