using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "export")]
public record GcloudComputeImagesExportOptions(
[property: CommandSwitch("--destination-uri")] string DestinationUri,
[property: CommandSwitch("--image")] string Image,
[property: CommandSwitch("--image-family")] string ImageFamily
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cloudbuild-service-account")]
    public string? CloudbuildServiceAccount { get; set; }

    [CommandSwitch("--compute-service-account")]
    public string? ComputeServiceAccount { get; set; }

    [CommandSwitch("--export-format")]
    public string? ExportFormat { get; set; }

    [CommandSwitch("--image-project")]
    public string? ImageProject { get; set; }

    [CommandSwitch("--log-location")]
    public string? LogLocation { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}