using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing PowerShell commands using the pwsh executable.
/// </summary>
[ExcludeFromCodeCoverage]
[CliTool("pwsh")]
public record PowerShellOptions : CommandLineToolOptions;
