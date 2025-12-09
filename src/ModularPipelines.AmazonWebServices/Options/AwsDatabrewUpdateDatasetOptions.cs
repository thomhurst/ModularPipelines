using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "update-dataset")]
public record AwsDatabrewUpdateDatasetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--input")] string Input
) : AwsOptions
{
    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--format-options")]
    public string? FormatOptions { get; set; }

    [CliOption("--path-options")]
    public string? PathOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}