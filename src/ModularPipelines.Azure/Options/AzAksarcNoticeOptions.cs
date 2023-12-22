using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aksarc", "notice")]
public record AzAksarcNoticeOptions(
[property: CommandSwitch("--output-filepath")] string OutputFilepath
) : AzOptions;