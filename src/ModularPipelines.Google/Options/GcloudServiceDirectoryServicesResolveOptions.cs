using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "services", "resolve")]
public record GcloudServiceDirectoryServicesResolveOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Namespace
) : GcloudOptions
{
    [CommandSwitch("--endpoint-filter")]
    public string? EndpointFilter { get; set; }

    [CommandSwitch("--max-endpoints")]
    public string? MaxEndpoints { get; set; }
}