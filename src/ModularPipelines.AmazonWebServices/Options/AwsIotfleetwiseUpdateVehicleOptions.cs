using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "update-vehicle")]
public record AwsIotfleetwiseUpdateVehicleOptions(
[property: CommandSwitch("--vehicle-name")] string VehicleName
) : AwsOptions
{
    [CommandSwitch("--model-manifest-arn")]
    public string? ModelManifestArn { get; set; }

    [CommandSwitch("--decoder-manifest-arn")]
    public string? DecoderManifestArn { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--attribute-update-mode")]
    public string? AttributeUpdateMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}