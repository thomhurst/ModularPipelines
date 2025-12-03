using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-https-proxies", "export")]
public record GcloudComputeTargetHttpsProxiesExportOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}