using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-thin-client", "update-environment")]
public record AwsWorkspacesThinClientUpdateEnvironmentOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--desktop-arn")]
    public string? DesktopArn { get; set; }

    [CliOption("--desktop-endpoint")]
    public string? DesktopEndpoint { get; set; }

    [CliOption("--software-set-update-schedule")]
    public string? SoftwareSetUpdateSchedule { get; set; }

    [CliOption("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CliOption("--software-set-update-mode")]
    public string? SoftwareSetUpdateMode { get; set; }

    [CliOption("--desired-software-set-id")]
    public string? DesiredSoftwareSetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}