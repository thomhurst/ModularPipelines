using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "lint")]
public record AzBicepLintOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--diagnostics-format")]
    public string? DiagnosticsFormat { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }
}