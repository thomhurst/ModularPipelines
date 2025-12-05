using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-dimension")]
public record AwsIotUpdateDimensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--string-values")] string[] StringValues
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}