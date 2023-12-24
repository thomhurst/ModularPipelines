using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "get-organization-custom-rule-policy")]
public record AwsConfigserviceGetOrganizationCustomRulePolicyOptions(
[property: CommandSwitch("--organization-config-rule-name")] string OrganizationConfigRuleName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}