using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "check-name")]
public record AzAcrCheckNameOptions(
[property: CliOption("--name")] string Name
) : AzOptions;