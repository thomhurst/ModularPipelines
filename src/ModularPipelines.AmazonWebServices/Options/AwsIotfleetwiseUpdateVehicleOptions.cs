using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "update-vehicle")]
public record AwsIotfleetwiseUpdateVehicleOptions(
[property: CliOption("--vehicle-name")] string VehicleName
) : AwsOptions
{
    [CliOption("--model-manifest-arn")]
    public string? ModelManifestArn { get; set; }

    [CliOption("--decoder-manifest-arn")]
    public string? DecoderManifestArn { get; set; }

    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--attribute-update-mode")]
    public string? AttributeUpdateMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}