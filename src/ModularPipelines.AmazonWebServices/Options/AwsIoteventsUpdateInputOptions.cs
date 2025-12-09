using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "update-input")]
public record AwsIoteventsUpdateInputOptions(
[property: CliOption("--input-name")] string InputName,
[property: CliOption("--input-definition")] string InputDefinition
) : AwsOptions
{
    [CliOption("--input-description")]
    public string? InputDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}