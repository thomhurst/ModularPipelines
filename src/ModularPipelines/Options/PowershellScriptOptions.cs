using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

public partial record PowershellScriptOptions([property: CliOption("-Command")] string Script) : PowershellOptions;