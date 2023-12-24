using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "get-compliance-details-by-config-rule")]
public record AwsConfigserviceGetComplianceDetailsByConfigRuleOptions(
[property: CommandSwitch("--config-rule-name")] string ConfigRuleName
) : AwsOptions
{
    [CommandSwitch("--compliance-types")]
    public string[]? ComplianceTypes { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}