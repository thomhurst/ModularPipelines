using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-services", "import")]
public record GcloudComputeBackendServicesImportOptions(
[property: PositionalArgument] string BackendServiceName
) : GcloudOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}