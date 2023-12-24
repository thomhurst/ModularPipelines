using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "update-rule-metadata")]
public record AwsFrauddetectorUpdateRuleMetadataOptions(
[property: CommandSwitch("--rule")] string Rule,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}