using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "manifest", "list-metadata")]
public record AzAcrManifestListMetadataOptions : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--orderby")]
    public string? Orderby { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--registry")]
    public string? Registry { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? REPOID { get; set; }
}