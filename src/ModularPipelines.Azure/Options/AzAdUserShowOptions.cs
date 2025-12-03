using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "user", "show")]
public record AzAdUserShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions;