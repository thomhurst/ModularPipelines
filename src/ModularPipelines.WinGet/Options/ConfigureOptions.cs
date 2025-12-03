using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configure")]
public record ConfigureOptions(
    [property: CliOption("--file")] string File
) : WingetOptions;