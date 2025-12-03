using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "show")]
public record AzAdAppShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions;