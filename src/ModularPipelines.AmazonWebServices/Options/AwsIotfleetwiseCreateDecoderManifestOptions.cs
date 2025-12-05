using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "create-decoder-manifest")]
public record AwsIotfleetwiseCreateDecoderManifestOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--model-manifest-arn")] string ModelManifestArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--signal-decoders")]
    public string[]? SignalDecoders { get; set; }

    [CliOption("--network-interfaces")]
    public string[]? NetworkInterfaces { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}