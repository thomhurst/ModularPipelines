using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "update-file-system")]
public record AwsEfsUpdateFileSystemOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId
) : AwsOptions
{
    [CommandSwitch("--throughput-mode")]
    public string? ThroughputMode { get; set; }

    [CommandSwitch("--provisioned-throughput-in-mibps")]
    public double? ProvisionedThroughputInMibps { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}