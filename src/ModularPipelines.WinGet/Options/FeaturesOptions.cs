using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("features")]
public record FeaturesOptions : WingetOptions;