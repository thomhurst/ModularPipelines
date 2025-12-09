using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("extension", "show")]
public record AzExtensionShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;