using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-thin-client", "create-environment")]
public record AwsWorkspacesThinClientCreateEnvironmentOptions(
[property: CliOption("--desktop-arn")] string DesktopArn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

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

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}