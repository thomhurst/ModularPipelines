using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "url-lists", "list")]
public record GcloudNetworkSecurityUrlListsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;