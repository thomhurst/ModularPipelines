using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Mac;

[ExcludeFromCodeCoverage]
[CliTool("brew")]
public record MacBrewOptions([property: CliOption("--cask")] string PackageName) : CommandLineToolOptions;