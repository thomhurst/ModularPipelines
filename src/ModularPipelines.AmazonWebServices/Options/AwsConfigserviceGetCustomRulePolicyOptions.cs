using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "get-custom-rule-policy")]
public record AwsConfigserviceGetCustomRulePolicyOptions : AwsOptions
{
    [CommandSwitch("--config-rule-name")]
    public string? ConfigRuleName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}