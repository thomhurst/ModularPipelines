using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-services", "remove-service-bindings")]
public record GcloudComputeBackendServicesRemoveServiceBindingsOptions(
[property: PositionalArgument] string BackendServiceName,
[property: CommandSwitch("--service-bindings")] string[] ServiceBindings
) : GcloudOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}