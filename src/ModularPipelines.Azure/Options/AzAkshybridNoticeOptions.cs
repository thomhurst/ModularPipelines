using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("akshybrid", "notice")]
public record AzAkshybridNoticeOptions(
[property: CliOption("--output-filepath")] string OutputFilepath
) : AzOptions;