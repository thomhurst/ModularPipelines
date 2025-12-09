using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing a PowerShell script file.
/// </summary>
/// <param name="FilePath">The path to the PowerShell script file to execute.</param>
[ExcludeFromCodeCoverage]
public record PowershellFileOptions([property: CliOption("-File")] string FilePath) : PowershellOptions;