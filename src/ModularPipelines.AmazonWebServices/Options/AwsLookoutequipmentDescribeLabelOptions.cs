using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "describe-label")]
public record AwsLookoutequipmentDescribeLabelOptions(
[property: CliOption("--label-group-name")] string LabelGroupName,
[property: CliOption("--label-id")] string LabelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}