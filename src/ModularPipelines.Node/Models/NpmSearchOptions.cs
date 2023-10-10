using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("search")]
public record NpmSearchOptions : NpmOptions
{
    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public bool? Parseable { get; set; }

    [BooleanCommandSwitch("--description")]
    public bool? Description { get; set; }

    [CommandSwitch("--searchopts")]
    public string? Searchopts { get; set; }

    [CommandSwitch("--searchexclude")]
    public string? Searchexclude { get; set; }

    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [BooleanCommandSwitch("--prefer-online")]
    public bool? PreferOnline { get; set; }

    [BooleanCommandSwitch("--prefer-offline")]
    public bool? PreferOffline { get; set; }

    [BooleanCommandSwitch("--offline")]
    public bool? Offline { get; set; }

}