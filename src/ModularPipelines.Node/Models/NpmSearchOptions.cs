using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("search", "terms")]
public record NpmSearchOptions
    (
        [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value
        ) : NpmOptions
{
    [BooleanCommandSwitch("--long")]
    public virtual bool? Long { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [CommandSwitch("--color")]
    public virtual string? Color { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public virtual bool? Parseable { get; set; }

    [BooleanCommandSwitch("--description")]
    public virtual bool? Description { get; set; }

    [CommandSwitch("--searchopts")]
    public virtual string? Searchopts { get; set; }

    [CommandSwitch("--searchexclude")]
    public virtual string? Searchexclude { get; set; }

    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [BooleanCommandSwitch("--prefer-online")]
    public virtual bool? PreferOnline { get; set; }

    [BooleanCommandSwitch("--prefer-offline")]
    public virtual bool? PreferOffline { get; set; }

    [BooleanCommandSwitch("--offline")]
    public virtual bool? Offline { get; set; }
}