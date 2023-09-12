using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
public record NpmOptions() : CommandLineToolOptions("npm");