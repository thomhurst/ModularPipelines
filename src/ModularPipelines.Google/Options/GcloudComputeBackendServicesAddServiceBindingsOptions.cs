using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "add-service-bindings")]
public record GcloudComputeBackendServicesAddServiceBindingsOptions(
[property: CliArgument] string BackendServiceName,
[property: CliOption("--service-bindings")] string[] ServiceBindings
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}