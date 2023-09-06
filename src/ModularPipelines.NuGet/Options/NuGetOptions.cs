using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Options;

namespace ModularPipelines.NuGet.Options;

[ExcludeFromCodeCoverage]
public record NuGetOptions() : CommandLineToolOptions("dotnet");
