using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("akshybrid", "notice")]
public record AzAkshybridNoticeOptions(
[property: CommandSwitch("--output-filepath")] string OutputFilepath
) : AzOptions;