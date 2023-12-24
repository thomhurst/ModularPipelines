using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "delete-snapshot-copy-configuration")]
public record AwsRedshiftServerlessDeleteSnapshotCopyConfigurationOptions(
[property: CommandSwitch("--snapshot-copy-configuration-id")] string SnapshotCopyConfigurationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}