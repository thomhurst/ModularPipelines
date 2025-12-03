using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("setapikey")]
public record SetApiKeyOptions : ChocoOptions
{
    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliOption("--api-key")]
    public virtual string? ApiKey { get; set; }
}