using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("anthos", "config", "controller", "get-credentials")]
public record GcloudAnthosConfigControllerGetCredentialsOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;