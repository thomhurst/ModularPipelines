using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tag", "remove-value")]
public record AzTagRemoveValueOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--value")] string Value
) : AzOptions;