using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "update-sampling-rule")]
public record AwsXrayUpdateSamplingRuleOptions(
[property: CliOption("--sampling-rule-update")] string SamplingRuleUpdate
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}