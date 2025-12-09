using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "delete")]
public record AzAdAppDeleteOptions(
[property: CliOption("--id")] string Id
) : AzOptions;