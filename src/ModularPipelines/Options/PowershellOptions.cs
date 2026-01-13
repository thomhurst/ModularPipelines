using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing PowerShell commands using the pwsh executable.
/// </summary>
[CliTool("pwsh")]
public partial record PowershellOptions : CommandLineToolOptions;