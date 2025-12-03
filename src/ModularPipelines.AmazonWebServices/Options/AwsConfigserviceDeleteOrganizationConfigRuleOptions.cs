using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-organization-config-rule")]
public record AwsConfigserviceDeleteOrganizationConfigRuleOptions(
[property: CliOption("--organization-config-rule-name")] string OrganizationConfigRuleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}