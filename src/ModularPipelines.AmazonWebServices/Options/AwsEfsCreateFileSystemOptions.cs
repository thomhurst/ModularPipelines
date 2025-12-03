using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "create-file-system")]
public record AwsEfsCreateFileSystemOptions : AwsOptions
{
    [CliOption("--creation-token")]
    public string? CreationToken { get; set; }

    [CliOption("--performance-mode")]
    public string? PerformanceMode { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--throughput-mode")]
    public string? ThroughputMode { get; set; }

    [CliOption("--provisioned-throughput-in-mibps")]
    public double? ProvisionedThroughputInMibps { get; set; }

    [CliOption("--availability-zone-name")]
    public string? AvailabilityZoneName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}