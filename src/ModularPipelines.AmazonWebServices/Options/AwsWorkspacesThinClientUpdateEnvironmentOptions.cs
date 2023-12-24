using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-thin-client", "update-environment")]
public record AwsWorkspacesThinClientUpdateEnvironmentOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--desktop-arn")]
    public string? DesktopArn { get; set; }

    [CommandSwitch("--desktop-endpoint")]
    public string? DesktopEndpoint { get; set; }

    [CommandSwitch("--software-set-update-schedule")]
    public string? SoftwareSetUpdateSchedule { get; set; }

    [CommandSwitch("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CommandSwitch("--software-set-update-mode")]
    public string? SoftwareSetUpdateMode { get; set; }

    [CommandSwitch("--desired-software-set-id")]
    public string? DesiredSoftwareSetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}