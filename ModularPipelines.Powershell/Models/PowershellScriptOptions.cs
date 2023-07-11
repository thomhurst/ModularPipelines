using ModularPipelines.Attributes;

namespace ModularPipelines.Powershell.Models;

public record PowershellScriptOptions([property: CommandSwitch("-Command")] string Script) : PowershellOptions;
