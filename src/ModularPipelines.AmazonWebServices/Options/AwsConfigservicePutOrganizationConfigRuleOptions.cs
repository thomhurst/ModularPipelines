using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-organization-config-rule")]
public record AwsConfigservicePutOrganizationConfigRuleOptions(
[property: CommandSwitch("--organization-config-rule-name")] string OrganizationConfigRuleName
) : AwsOptions
{
    [CommandSwitch("--organization-managed-rule-metadata")]
    public string? OrganizationManagedRuleMetadata { get; set; }

    [CommandSwitch("--organization-custom-rule-metadata")]
    public string? OrganizationCustomRuleMetadata { get; set; }

    [CommandSwitch("--excluded-accounts")]
    public string[]? ExcludedAccounts { get; set; }

    [CommandSwitch("--organization-custom-policy-rule-metadata")]
    public string? OrganizationCustomPolicyRuleMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}