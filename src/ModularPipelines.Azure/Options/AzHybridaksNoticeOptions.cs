using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hybridaks", "notice")]
public record AzHybridaksNoticeOptions(
[property: CommandSwitch("--output-filepath")] string OutputFilepath
) : AzOptions;