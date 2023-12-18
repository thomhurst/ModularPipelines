using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configure")]
public record ConfigureOptions(
    [property: CommandSwitch("--file")] string File
) : WingetOptions
{
}