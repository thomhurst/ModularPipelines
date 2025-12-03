using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-organization-custom-rule-policy")]
public record AwsConfigserviceGetOrganizationCustomRulePolicyOptions(
[property: CliOption("--organization-config-rule-name")] string OrganizationConfigRuleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}