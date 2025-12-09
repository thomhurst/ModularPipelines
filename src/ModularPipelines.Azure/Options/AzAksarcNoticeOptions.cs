using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aksarc", "notice")]
public record AzAksarcNoticeOptions(
[property: CliOption("--output-filepath")] string OutputFilepath
) : AzOptions;