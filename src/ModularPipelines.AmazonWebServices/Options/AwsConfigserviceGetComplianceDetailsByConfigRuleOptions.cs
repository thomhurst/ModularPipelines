using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-compliance-details-by-config-rule")]
public record AwsConfigserviceGetComplianceDetailsByConfigRuleOptions(
[property: CliOption("--config-rule-name")] string ConfigRuleName
) : AwsOptions
{
    [CliOption("--compliance-types")]
    public string[]? ComplianceTypes { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}