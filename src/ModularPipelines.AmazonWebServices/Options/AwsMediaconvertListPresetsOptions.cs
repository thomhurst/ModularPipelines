using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconvert", "list-presets")]
public record AwsMediaconvertListPresetsOptions : AwsOptions
{
    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--list-by")]
    public string? ListBy { get; set; }

    [CliOption("--order")]
    public string? Order { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}