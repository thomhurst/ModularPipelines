using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record PowershellFileOptions([property: CommandSwitch("-File")] string FilePath) : PowershellOptions;