using ModularPipelines.Attributes;

namespace ModularPipelines.Powershell.Models;

public record PowershellFileOptions([property: CommandSwitch("-File")] string FilePath) : PowershellOptions;
