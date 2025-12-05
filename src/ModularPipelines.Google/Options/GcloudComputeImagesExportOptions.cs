using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "export")]
public record GcloudComputeImagesExportOptions(
[property: CliOption("--destination-uri")] string DestinationUri,
[property: CliOption("--image")] string Image,
[property: CliOption("--image-family")] string ImageFamily
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cloudbuild-service-account")]
    public string? CloudbuildServiceAccount { get; set; }

    [CliOption("--compute-service-account")]
    public string? ComputeServiceAccount { get; set; }

    [CliOption("--export-format")]
    public string? ExportFormat { get; set; }

    [CliOption("--image-project")]
    public string? ImageProject { get; set; }

    [CliOption("--log-location")]
    public string? LogLocation { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}