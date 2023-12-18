using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "manifest", "metadata", "update")]
public record AzAcrManifestMetadataUpdateOptions : AzOptions
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
    public string? ManifestId { get; set; }
}