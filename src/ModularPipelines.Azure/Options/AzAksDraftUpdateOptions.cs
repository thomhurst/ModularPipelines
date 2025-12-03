using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "draft", "update")]
public record AzAksDraftUpdateOptions : AzOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }
}