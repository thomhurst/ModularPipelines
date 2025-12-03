using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "delete-replication-configuration")]
public record AwsEfsDeleteReplicationConfigurationOptions(
[property: CliOption("--source-file-system-id")] string SourceFileSystemId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}