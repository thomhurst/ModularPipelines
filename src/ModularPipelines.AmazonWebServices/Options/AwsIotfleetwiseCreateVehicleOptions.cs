using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "create-vehicle")]
public record AwsIotfleetwiseCreateVehicleOptions(
[property: CommandSwitch("--vehicle-name")] string VehicleName,
[property: CommandSwitch("--model-manifest-arn")] string ModelManifestArn,
[property: CommandSwitch("--decoder-manifest-arn")] string DecoderManifestArn
) : AwsOptions
{
    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--association-behavior")]
    public string? AssociationBehavior { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}