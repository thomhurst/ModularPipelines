using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-thin-client", "update-device")]
public record AwsWorkspacesThinClientUpdateDeviceOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--desired-software-set-id")]
    public string? DesiredSoftwareSetId { get; set; }

    [CliOption("--software-set-update-schedule")]
    public string? SoftwareSetUpdateSchedule { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}