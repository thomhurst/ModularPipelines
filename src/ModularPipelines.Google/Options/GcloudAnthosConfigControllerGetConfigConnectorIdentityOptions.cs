using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "config", "controller", "get-config-connector-identity")]
public record GcloudAnthosConfigControllerGetConfigConnectorIdentityOptions(
[property: CliArgument] string Name,
[property: CliOption("--location")] string Location
) : GcloudOptions;