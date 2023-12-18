using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "manifest", "restore")]
public record AzAcrManifestRestoreOptions : AzOptions
{
    [CommandSwitch("--digest")]
    public string? Digest { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--registry")]
    public string? Registry { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ManifestId { get; set; }
}

