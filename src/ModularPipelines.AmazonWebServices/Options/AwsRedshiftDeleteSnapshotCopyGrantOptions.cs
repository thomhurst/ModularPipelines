using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-snapshot-copy-grant")]
public record AwsRedshiftDeleteSnapshotCopyGrantOptions(
[property: CliOption("--snapshot-copy-grant-name")] string SnapshotCopyGrantName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}