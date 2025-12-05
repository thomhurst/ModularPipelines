using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "copy-snapshot-and-update-volume")]
public record AwsFsxCopySnapshotAndUpdateVolumeOptions(
[property: CliOption("--volume-id")] string VolumeId,
[property: CliOption("--source-snapshot-arn")] string SourceSnapshotArn
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--copy-strategy")]
    public string? CopyStrategy { get; set; }

    [CliOption("--options")]
    public string[]? Options { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}