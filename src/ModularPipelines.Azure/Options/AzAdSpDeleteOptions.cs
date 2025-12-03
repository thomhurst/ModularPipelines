using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "sp", "delete")]
public record AzAdSpDeleteOptions(
[property: CliOption("--id")] string Id
) : AzOptions;