using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "check-name")]
public record AzApimCheckNameOptions(
[property: CliOption("--name")] string Name
) : AzOptions;