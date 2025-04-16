using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fund")]
public record NpmFundOptions : NpmOptions
{
    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [CommandSwitch("--browser")]
    public virtual string? Browser { get; set; }

    [BooleanCommandSwitch("--unicode")]
    public virtual bool? Unicode { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CommandSwitch("--which")]
    public virtual int? Which { get; set; }
}