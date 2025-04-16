using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("push")]
public record PushOptions(
    [property: PositionalArgument] string Pkg
) : ChocoOptions
{
    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [CommandSwitch("--api-key")]
    public virtual string? ApiKey { get; set; }

    [CommandSwitch("--code")]
    public virtual string? Code { get; set; }

    [CommandSwitch("--redirecturl")]
    public virtual string? Redirecturl { get; set; }

    [CommandSwitch("--endpoint")]
    public virtual string? Endpoint { get; set; }

    [BooleanCommandSwitch("--skip-cleanup")]
    public virtual bool? SkipCleanup { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}