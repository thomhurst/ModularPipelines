using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "ssl", "list")]
public record AzFunctionappConfigSslListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;