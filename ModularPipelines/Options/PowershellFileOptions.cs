using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

public record PowershellFileOptions([property: CommandSwitch("-File")] string FilePath) : PowershellOptions;
