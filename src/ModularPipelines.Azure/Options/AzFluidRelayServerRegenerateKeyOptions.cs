using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fluid-relay", "server", "regenerate-key")]
public record AzFluidRelayServerRegenerateKeyOptions(
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName
) : AzOptions;