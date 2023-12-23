using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "create-decoder-manifest")]
public record AwsIotfleetwiseCreateDecoderManifestOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--model-manifest-arn")] string ModelManifestArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--signal-decoders")]
    public string[]? SignalDecoders { get; set; }

    [CommandSwitch("--network-interfaces")]
    public string[]? NetworkInterfaces { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}