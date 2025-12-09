using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("config")]
public record ConfigOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--value")]
    public virtual string? Value { get; set; }
}