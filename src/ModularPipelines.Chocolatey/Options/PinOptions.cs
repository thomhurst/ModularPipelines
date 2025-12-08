using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pin")]
public record PinOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliOption("--note")]
    public virtual string? Note { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}