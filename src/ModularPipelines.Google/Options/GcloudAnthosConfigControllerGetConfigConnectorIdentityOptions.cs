using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("anthos", "config", "controller", "get-config-connector-identity")]
public record GcloudAnthosConfigControllerGetConfigConnectorIdentityOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;