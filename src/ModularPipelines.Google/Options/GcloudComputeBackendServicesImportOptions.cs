using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "import")]
public record GcloudComputeBackendServicesImportOptions(
[property: CliArgument] string BackendServiceName
) : GcloudOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}