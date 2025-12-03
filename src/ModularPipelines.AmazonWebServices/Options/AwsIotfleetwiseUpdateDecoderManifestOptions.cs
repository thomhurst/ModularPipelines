using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "update-decoder-manifest")]
public record AwsIotfleetwiseUpdateDecoderManifestOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--signal-decoders-to-add")]
    public string[]? SignalDecodersToAdd { get; set; }

    [CliOption("--signal-decoders-to-update")]
    public string[]? SignalDecodersToUpdate { get; set; }

    [CliOption("--signal-decoders-to-remove")]
    public string[]? SignalDecodersToRemove { get; set; }

    [CliOption("--network-interfaces-to-add")]
    public string[]? NetworkInterfacesToAdd { get; set; }

    [CliOption("--network-interfaces-to-update")]
    public string[]? NetworkInterfacesToUpdate { get; set; }

    [CliOption("--network-interfaces-to-remove")]
    public string[]? NetworkInterfacesToRemove { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}