using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-aggregate-compliance-details-by-config-rule")]
public record AwsConfigserviceGetAggregateComplianceDetailsByConfigRuleOptions(
[property: CliOption("--configuration-aggregator-name")] string ConfigurationAggregatorName,
[property: CliOption("--config-rule-name")] string ConfigRuleName,
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--aws-region")] string AwsRegion
) : AwsOptions
{
    [CliOption("--compliance-type")]
    public string? ComplianceType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}