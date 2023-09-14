using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[CommandPrecedingArguments("install")]
[ExcludeFromCodeCoverage]
public record NpmInstallOptions : NpmOptions
{
    [PositionalArgument]
    public string? Target { get; init; }

    [BooleanCommandSwitch("--global")]
    public bool Global { get; init; }

    [BooleanCommandSwitch("--dry-run")]
    public bool DryRun { get; init; }

    [BooleanCommandSwitch("--force")]
    public bool Force { get; init; }

    [BooleanCommandSwitch("--save-dev")]
    public bool SaveDev { get; set; }
}
