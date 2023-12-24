using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "list-organization-insights")]
public record AwsDevopsGuruListOrganizationInsightsOptions(
[property: CommandSwitch("--status-filter")] string StatusFilter
) : AwsOptions
{
    [CommandSwitch("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CommandSwitch("--organizational-unit-ids")]
    public string[]? OrganizationalUnitIds { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}