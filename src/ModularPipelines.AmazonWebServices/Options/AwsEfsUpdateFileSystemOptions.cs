using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "update-file-system")]
public record AwsEfsUpdateFileSystemOptions(
[property: CliOption("--file-system-id")] string FileSystemId
) : AwsOptions
{
    [CliOption("--throughput-mode")]
    public string? ThroughputMode { get; set; }

    [CliOption("--provisioned-throughput-in-mibps")]
    public double? ProvisionedThroughputInMibps { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}