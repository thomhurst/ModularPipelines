using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("constraints")]
public record YarnConstraintsOptions : YarnOptions
{
    [BooleanCommandSwitch("--fix")]
    public virtual bool? Fix { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }
}