using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "sp", "create")]
public record AzAdSpCreateOptions(
[property: CliOption("--id")] string Id
) : AzOptions;