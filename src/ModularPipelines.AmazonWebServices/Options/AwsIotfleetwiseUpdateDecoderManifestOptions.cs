using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "update-decoder-manifest")]
public record AwsIotfleetwiseUpdateDecoderManifestOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--signal-decoders-to-add")]
    public string[]? SignalDecodersToAdd { get; set; }

    [CommandSwitch("--signal-decoders-to-update")]
    public string[]? SignalDecodersToUpdate { get; set; }

    [CommandSwitch("--signal-decoders-to-remove")]
    public string[]? SignalDecodersToRemove { get; set; }

    [CommandSwitch("--network-interfaces-to-add")]
    public string[]? NetworkInterfacesToAdd { get; set; }

    [CommandSwitch("--network-interfaces-to-update")]
    public string[]? NetworkInterfacesToUpdate { get; set; }

    [CommandSwitch("--network-interfaces-to-remove")]
    public string[]? NetworkInterfacesToRemove { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}