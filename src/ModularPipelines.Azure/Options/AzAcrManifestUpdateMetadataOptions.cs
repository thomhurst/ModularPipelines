using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "manifest", "update-metadata")]
public record AzAcrManifestUpdateMetadataOptions : AzOptions
{
    [BooleanCommandSwitch("--delete-enabled")]
    public bool? DeleteEnabled { get; set; }

    [BooleanCommandSwitch("--list-enabled")]
    public bool? ListEnabled { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--read-enabled")]
    public bool? ReadEnabled { get; set; }

    [CommandSwitch("--registry")]
    public string? Registry { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [BooleanCommandSwitch("--write-enabled")]
    public bool? WriteEnabled { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? MANIFESTID { get; set; }
}