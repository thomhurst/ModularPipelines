using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("doctor")]
public record NpmDoctorOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

}