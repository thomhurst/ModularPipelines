using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "describe-organization-health")]
public record AwsDevopsGuruDescribeOrganizationHealthOptions : AwsOptions
{
    [CommandSwitch("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CommandSwitch("--organizational-unit-ids")]
    public string[]? OrganizationalUnitIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}