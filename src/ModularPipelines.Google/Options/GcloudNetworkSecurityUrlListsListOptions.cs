using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "url-lists", "list")]
public record GcloudNetworkSecurityUrlListsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;