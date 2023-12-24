using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "copy-snapshot-and-update-volume")]
public record AwsFsxCopySnapshotAndUpdateVolumeOptions(
[property: CommandSwitch("--volume-id")] string VolumeId,
[property: CommandSwitch("--source-snapshot-arn")] string SourceSnapshotArn
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--copy-strategy")]
    public string? CopyStrategy { get; set; }

    [CommandSwitch("--options")]
    public string[]? Options { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}