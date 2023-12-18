using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
public record AzOptions() : CommandLineToolOptions("az");
