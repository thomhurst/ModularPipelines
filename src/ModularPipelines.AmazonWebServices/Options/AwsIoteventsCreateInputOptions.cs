using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "create-input")]
public record AwsIoteventsCreateInputOptions(
[property: CliOption("--input-name")] string InputName,
[property: CliOption("--input-definition")] string InputDefinition
) : AwsOptions
{
    [CliOption("--input-description")]
    public string? InputDescription { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}