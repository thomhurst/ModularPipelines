using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing Bash commands using the bash executable.
/// </summary>
[ExcludeFromCodeCoverage]
public record BashOptions() : CommandLineToolOptions("bash");
