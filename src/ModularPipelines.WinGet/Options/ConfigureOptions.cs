using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("configure")]
public record ConfigureOptions(
    [property: CliOption("--file")] string File
) : WingetOptions;