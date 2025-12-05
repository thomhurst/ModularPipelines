using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "update-label-group")]
public record AwsLookoutequipmentUpdateLabelGroupOptions(
[property: CliOption("--label-group-name")] string LabelGroupName
) : AwsOptions
{
    [CliOption("--fault-codes")]
    public string[]? FaultCodes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}