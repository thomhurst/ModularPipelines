using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "configure-docker")]
public record GcloudAuthConfigureDockerOptions(
[property: CliArgument] string Registries
) : GcloudOptions;