using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "configure-docker")]
public record GcloudAuthConfigureDockerOptions(
[property: PositionalArgument] string Registries
) : GcloudOptions;