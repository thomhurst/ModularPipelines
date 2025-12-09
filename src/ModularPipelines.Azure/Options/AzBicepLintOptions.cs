using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bicep", "lint")]
public record AzBicepLintOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliOption("--diagnostics-format")]
    public string? DiagnosticsFormat { get; set; }

    [CliFlag("--no-restore")]
    public bool? NoRestore { get; set; }
}