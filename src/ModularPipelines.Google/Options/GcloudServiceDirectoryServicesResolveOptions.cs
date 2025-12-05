using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "services", "resolve")]
public record GcloudServiceDirectoryServicesResolveOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliArgument] string Namespace
) : GcloudOptions
{
    [CliOption("--endpoint-filter")]
    public string? EndpointFilter { get; set; }

    [CliOption("--max-endpoints")]
    public string? MaxEndpoints { get; set; }
}