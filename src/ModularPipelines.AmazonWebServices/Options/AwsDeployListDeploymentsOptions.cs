using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "list-deployments")]
public record AwsDeployListDeploymentsOptions : AwsOptions
{
    [CliOption("--application-name")]
    public string? ApplicationName { get; set; }

    [CliOption("--deployment-group-name")]
    public string? DeploymentGroupName { get; set; }

    [CliOption("--external-id")]
    public string? ExternalId { get; set; }

    [CliOption("--include-only-statuses")]
    public string[]? IncludeOnlyStatuses { get; set; }

    [CliOption("--create-time-range")]
    public string? CreateTimeRange { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}