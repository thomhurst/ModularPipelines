using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing an inline PowerShell script.
/// </summary>
/// <param name="Script">The PowerShell script to execute.</param>
[ExcludeFromCodeCoverage]
public record PowerShellScriptOptions([property: CliOption("-Command")] string Script) : PowerShellOptions;
