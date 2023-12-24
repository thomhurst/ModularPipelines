using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-snapshot-copy-grant")]
public record AwsRedshiftDeleteSnapshotCopyGrantOptions(
[property: CommandSwitch("--snapshot-copy-grant-name")] string SnapshotCopyGrantName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}