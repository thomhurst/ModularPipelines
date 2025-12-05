using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Mac;

[ExcludeFromCodeCoverage]
public record MacBrewOptions([property: CliOption("--cask")] string PackageName) : CommandLineToolOptions("brew");