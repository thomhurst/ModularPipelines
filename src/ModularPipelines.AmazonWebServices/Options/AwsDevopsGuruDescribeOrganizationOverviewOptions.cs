using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "describe-organization-overview")]
public record AwsDevopsGuruDescribeOrganizationOverviewOptions(
[property: CommandSwitch("--from-time")] long FromTime
) : AwsOptions
{
    [CommandSwitch("--to-time")]
    public long? ToTime { get; set; }

    [CommandSwitch("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CommandSwitch("--organizational-unit-ids")]
    public string[]? OrganizationalUnitIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}