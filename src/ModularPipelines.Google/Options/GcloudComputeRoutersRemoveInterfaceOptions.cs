using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "remove-interface")]
public record GcloudComputeRoutersRemoveInterfaceOptions(
[property: CliArgument] string Name,
[property: CliOption("--interface-name")] string InterfaceName,
[property: CliOption("--interface-names")] string[] InterfaceNames
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}