using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record BashOptions() : CommandLineToolOptions("bash");
