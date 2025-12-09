using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("extension", "list-versions")]
public record AzExtensionListVersionsOptions(
[property: CliOption("--name")] string Name
) : AzOptions;