using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "group", "delete")]
public record AzAdGroupDeleteOptions(
[property: CliOption("--group")] string Group
) : AzOptions;