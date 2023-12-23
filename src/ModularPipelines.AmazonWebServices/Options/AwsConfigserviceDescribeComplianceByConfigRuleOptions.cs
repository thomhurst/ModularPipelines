using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "describe-compliance-by-config-rule")]
public record AwsConfigserviceDescribeComplianceByConfigRuleOptions : AwsOptions
{
    [CommandSwitch("--config-rule-names")]
    public string[]? ConfigRuleNames { get; set; }

    [CommandSwitch("--compliance-types")]
    public string[]? ComplianceTypes { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}