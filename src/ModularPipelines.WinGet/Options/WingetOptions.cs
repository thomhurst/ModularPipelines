using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Options;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
public record WingetOptions() : CommandLineToolOptions("winget")
{
}