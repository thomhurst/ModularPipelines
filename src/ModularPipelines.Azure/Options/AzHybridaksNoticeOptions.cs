using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hybridaks", "notice")]
public record AzHybridaksNoticeOptions(
[property: CliOption("--output-filepath")] string OutputFilepath
) : AzOptions;