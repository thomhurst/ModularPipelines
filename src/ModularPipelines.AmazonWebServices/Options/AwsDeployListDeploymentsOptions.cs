using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "list-deployments")]
public record AwsDeployListDeploymentsOptions : AwsOptions
{
    [CommandSwitch("--application-name")]
    public string? ApplicationName { get; set; }

    [CommandSwitch("--deployment-group-name")]
    public string? DeploymentGroupName { get; set; }

    [CommandSwitch("--external-id")]
    public string? ExternalId { get; set; }

    [CommandSwitch("--include-only-statuses")]
    public string[]? IncludeOnlyStatuses { get; set; }

    [CommandSwitch("--create-time-range")]
    public string? CreateTimeRange { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}