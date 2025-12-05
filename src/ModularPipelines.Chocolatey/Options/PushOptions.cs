using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("push")]
public record PushOptions(
    [property: CliArgument] string Pkg
) : ChocoOptions
{
    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliOption("--api-key")]
    public virtual string? ApiKey { get; set; }

    [CliOption("--code")]
    public virtual string? Code { get; set; }

    [CliOption("--redirecturl")]
    public virtual string? Redirecturl { get; set; }

    [CliOption("--endpoint")]
    public virtual string? Endpoint { get; set; }

    [CliFlag("--skip-cleanup")]
    public virtual bool? SkipCleanup { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}