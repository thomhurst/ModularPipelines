using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fluid-relay", "server", "regenerate-key")]
public record AzFluidRelayServerRegenerateKeyOptions(
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions;