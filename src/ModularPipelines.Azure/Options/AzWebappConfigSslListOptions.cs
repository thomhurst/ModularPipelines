using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "ssl", "list")]
public record AzWebappConfigSslListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;