using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("extension", "remove")]
public record AzExtensionRemoveOptions(
[property: CliOption("--name")] string Name
) : AzOptions;