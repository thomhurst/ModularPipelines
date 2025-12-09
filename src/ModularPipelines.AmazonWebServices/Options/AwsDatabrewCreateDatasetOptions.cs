using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "create-dataset")]
public record AwsDatabrewCreateDatasetOptions(
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

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}