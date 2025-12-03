using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configure")]
public record AzConfigureOptions : AzOptions
{
    [CliOption("--defaults")]
    public string? Defaults { get; set; }

    [CliFlag("--list-defaults")]
    public bool? ListDefaults { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}