using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "config", "ssl", "list")]
public record AzFunctionappConfigSslListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;