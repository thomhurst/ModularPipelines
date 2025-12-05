using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "get")]
public record ConfigGetOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--value")]
    public virtual string? Value { get; set; }
}