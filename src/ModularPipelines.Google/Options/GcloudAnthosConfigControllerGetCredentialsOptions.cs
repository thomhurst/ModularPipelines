using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "config", "controller", "get-credentials")]
public record GcloudAnthosConfigControllerGetCredentialsOptions(
[property: CliArgument] string Name,
[property: CliOption("--location")] string Location
) : GcloudOptions;