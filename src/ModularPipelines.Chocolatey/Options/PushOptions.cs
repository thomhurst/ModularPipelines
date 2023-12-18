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
    public string? Source { get; set; }

    [CommandSwitch("--api-key")]
    public string? ApiKey { get; set; }

    [CommandSwitch("--code")]
    public string? Code { get; set; }

    [CommandSwitch("--redirecturl")]
    public string? Redirecturl { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [BooleanCommandSwitch("--skip-cleanup")]
    public bool? SkipCleanup { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}