using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tag", "add-value")]
public record AzTagAddValueOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--value")] string Value
) : AzOptions;