using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "service-bindings", "create")]
public record GcloudNetworkServicesServiceBindingsCreateOptions(
[property: PositionalArgument] string ServiceBinding,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--service-directory-namespace")] string ServiceDirectoryNamespace,
[property: CommandSwitch("--service-directory-region")] string ServiceDirectoryRegion,
[property: CommandSwitch("--service-directory-service")] string ServiceDirectoryService
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}