using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "create-file-system")]
public record AwsEfsCreateFileSystemOptions : AwsOptions
{
    [CommandSwitch("--creation-token")]
    public string? CreationToken { get; set; }

    [CommandSwitch("--performance-mode")]
    public string? PerformanceMode { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--throughput-mode")]
    public string? ThroughputMode { get; set; }

    [CommandSwitch("--provisioned-throughput-in-mibps")]
    public double? ProvisionedThroughputInMibps { get; set; }

    [CommandSwitch("--availability-zone-name")]
    public string? AvailabilityZoneName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}