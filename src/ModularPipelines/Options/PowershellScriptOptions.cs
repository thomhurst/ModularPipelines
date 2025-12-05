using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

public record PowershellScriptOptions([property: CliOption("-Command")] string Script) : PowershellOptions;