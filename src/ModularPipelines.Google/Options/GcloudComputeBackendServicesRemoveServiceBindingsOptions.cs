using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "remove-service-bindings")]
public record GcloudComputeBackendServicesRemoveServiceBindingsOptions(
[property: CliArgument] string BackendServiceName,
[property: CliOption("--service-bindings")] string[] ServiceBindings
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}