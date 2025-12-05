using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "service-bindings", "create")]
public record GcloudNetworkServicesServiceBindingsCreateOptions(
[property: CliArgument] string ServiceBinding,
[property: CliArgument] string Location,
[property: CliOption("--service-directory-namespace")] string ServiceDirectoryNamespace,
[property: CliOption("--service-directory-region")] string ServiceDirectoryRegion,
[property: CliOption("--service-directory-service")] string ServiceDirectoryService
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }
}