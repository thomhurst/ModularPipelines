using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

public record PowershellScriptOptions([property: CommandSwitch("-Command")] string Script) : PowershellOptions;
