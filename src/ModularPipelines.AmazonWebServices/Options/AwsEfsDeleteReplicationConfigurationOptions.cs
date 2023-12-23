using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "delete-replication-configuration")]
public record AwsEfsDeleteReplicationConfigurationOptions(
[property: CommandSwitch("--source-file-system-id")] string SourceFileSystemId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}