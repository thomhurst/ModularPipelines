using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "create-vehicle")]
public record AwsIotfleetwiseCreateVehicleOptions(
[property: CliOption("--vehicle-name")] string VehicleName,
[property: CliOption("--model-manifest-arn")] string ModelManifestArn,
[property: CliOption("--decoder-manifest-arn")] string DecoderManifestArn
) : AwsOptions
{
    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--association-behavior")]
    public string? AssociationBehavior { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}