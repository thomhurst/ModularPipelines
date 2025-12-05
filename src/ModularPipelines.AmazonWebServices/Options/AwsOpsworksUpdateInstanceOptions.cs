using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "update-instance")]
public record AwsOpsworksUpdateInstanceOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--layer-ids")]
    public string[]? LayerIds { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--auto-scaling-type")]
    public string? AutoScalingType { get; set; }

    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--os")]
    public string? Os { get; set; }

    [CliOption("--ami-id")]
    public string? AmiId { get; set; }

    [CliOption("--ssh-key-name")]
    public string? SshKeyName { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--agent-version")]
    public string? AgentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}