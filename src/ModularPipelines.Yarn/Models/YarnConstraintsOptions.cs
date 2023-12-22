using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("constraints")]
public record YarnConstraintsOptions : YarnOptions
{
    [BooleanCommandSwitch("--fix")]
    public bool? Fix { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }
}