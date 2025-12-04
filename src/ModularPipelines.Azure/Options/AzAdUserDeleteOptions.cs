using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "user", "delete")]
public record AzAdUserDeleteOptions(
[property: CliOption("--id")] string Id
) : AzOptions;