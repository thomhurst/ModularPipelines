using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "export")]
public record GcloudComputeBackendServicesExportOptions(
[property: CliArgument] string BackendServiceName
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}