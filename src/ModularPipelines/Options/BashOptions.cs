using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing Bash commands using the bash executable.
/// </summary>
[ExcludeFromCodeCoverage]
[CliTool("bash")]
public record BashOptions : CommandLineToolOptions;