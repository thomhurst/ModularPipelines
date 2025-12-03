using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "sp", "create")]
public record AzAdSpCreateOptions(
[property: CliOption("--id")] string Id
) : AzOptions;