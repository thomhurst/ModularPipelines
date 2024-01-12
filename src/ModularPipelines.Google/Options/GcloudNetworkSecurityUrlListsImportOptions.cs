using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "url-lists", "import")]
public record GcloudNetworkSecurityUrlListsImportOptions(
[property: PositionalArgument] string UrlList,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}