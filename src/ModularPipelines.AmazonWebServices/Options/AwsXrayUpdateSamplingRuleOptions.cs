using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("xray", "update-sampling-rule")]
public record AwsXrayUpdateSamplingRuleOptions(
[property: CommandSwitch("--sampling-rule-update")] string SamplingRuleUpdate
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}