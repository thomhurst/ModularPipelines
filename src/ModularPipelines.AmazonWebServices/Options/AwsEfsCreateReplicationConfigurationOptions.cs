using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "create-replication-configuration")]
public record AwsEfsCreateReplicationConfigurationOptions(
[property: CommandSwitch("--source-file-system-id")] string SourceFileSystemId,
[property: CommandSwitch("--destinations")] string[] Destinations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}