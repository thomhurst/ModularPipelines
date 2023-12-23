using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "generate-organizations-access-report")]
public record AwsIamGenerateOrganizationsAccessReportOptions(
[property: CommandSwitch("--entity-path")] string EntityPath
) : AwsOptions
{
    [CommandSwitch("--organizations-policy-id")]
    public string? OrganizationsPolicyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}