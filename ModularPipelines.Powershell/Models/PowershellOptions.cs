using ModularPipelines.Options;

namespace ModularPipelines.Powershell.Models;

public record PowershellOptions() : CommandLineToolOptions("pwsh");
