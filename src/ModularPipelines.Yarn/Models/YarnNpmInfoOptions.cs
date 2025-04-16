using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "info")]
public record YarnNpmInfoOptions : YarnOptions
{
    [CommandSwitch("--fields")]
    public virtual string? Fields { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }
}