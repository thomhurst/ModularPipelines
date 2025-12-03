using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("term", "accept")]
public record AzTermAcceptOptions(
[property: CliOption("--plan")] string Plan,
[property: CliOption("--product")] string Product,
[property: CliOption("--publisher")] string Publisher
) : AzOptions;