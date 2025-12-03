using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-organization-config-rule")]
public record AwsConfigservicePutOrganizationConfigRuleOptions(
[property: CliOption("--organization-config-rule-name")] string OrganizationConfigRuleName
) : AwsOptions
{
    [CliOption("--organization-managed-rule-metadata")]
    public string? OrganizationManagedRuleMetadata { get; set; }

    [CliOption("--organization-custom-rule-metadata")]
    public string? OrganizationCustomRuleMetadata { get; set; }

    [CliOption("--excluded-accounts")]
    public string[]? ExcludedAccounts { get; set; }

    [CliOption("--organization-custom-policy-rule-metadata")]
    public string? OrganizationCustomPolicyRuleMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}