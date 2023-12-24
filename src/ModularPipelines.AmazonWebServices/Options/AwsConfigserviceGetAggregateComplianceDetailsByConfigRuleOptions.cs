using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "get-aggregate-compliance-details-by-config-rule")]
public record AwsConfigserviceGetAggregateComplianceDetailsByConfigRuleOptions(
[property: CommandSwitch("--configuration-aggregator-name")] string ConfigurationAggregatorName,
[property: CommandSwitch("--config-rule-name")] string ConfigRuleName,
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--aws-region")] string AwsRegion
) : AwsOptions
{
    [CommandSwitch("--compliance-type")]
    public string? ComplianceType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}