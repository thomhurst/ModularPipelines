using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Options;

namespace ModularPipelines.Git.Options;

[ExcludeFromCodeCoverage]
public record GitOptions() : CommandLineToolOptions("git");
