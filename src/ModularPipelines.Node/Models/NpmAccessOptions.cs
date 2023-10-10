using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access")]
public record NpmAccessOptions : NpmOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }

    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }
}