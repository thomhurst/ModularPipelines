using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config")]
public record YarnConfigOptions : YarnOptions
{
    [BooleanCommandSwitch("--no-defaults")]
    public bool? NoDefaults { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }
}