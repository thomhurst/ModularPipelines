using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "check-name")]
public record AzApimCheckNameOptions(
[property: CliOption("--name")] string Name
) : AzOptions;