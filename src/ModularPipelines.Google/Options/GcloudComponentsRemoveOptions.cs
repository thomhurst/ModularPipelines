using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("components", "remove")]
public record GcloudComponentsRemoveOptions(
[property: CliArgument] string ComponentId
) : GcloudOptions;