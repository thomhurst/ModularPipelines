using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "create-replication-configuration")]
public record AwsEfsCreateReplicationConfigurationOptions(
[property: CliOption("--source-file-system-id")] string SourceFileSystemId,
[property: CliOption("--destinations")] string[] Destinations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}