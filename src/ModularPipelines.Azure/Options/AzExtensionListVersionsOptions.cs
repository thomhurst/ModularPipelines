using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("extension", "list-versions")]
public record AzExtensionListVersionsOptions(
[property: CliOption("--name")] string Name
) : AzOptions;