using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alias", "export")]
public record AzAliasExportOptions : AzOptions
{
    [CliOption("--exclude")]
    public string? Exclude { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }
}