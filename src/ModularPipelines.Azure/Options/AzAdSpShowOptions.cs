using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "sp", "show")]
public record AzAdSpShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions;