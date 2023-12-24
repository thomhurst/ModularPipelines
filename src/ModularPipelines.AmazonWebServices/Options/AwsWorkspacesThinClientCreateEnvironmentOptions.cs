using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-thin-client", "create-environment")]
public record AwsWorkspacesThinClientCreateEnvironmentOptions(
[property: CommandSwitch("--desktop-arn")] string DesktopArn
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

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

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}