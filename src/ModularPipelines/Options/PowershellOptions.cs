namespace ModularPipelines.Options;

/// <summary>
/// Options for executing PowerShell commands using the pwsh executable.
/// </summary>
public record PowershellOptions() : CommandLineToolOptions("pwsh");
