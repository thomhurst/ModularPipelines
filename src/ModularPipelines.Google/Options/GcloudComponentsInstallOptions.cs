using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("components", "install")]
public record GcloudComponentsInstallOptions(
[property: CliArgument] string ComponentIds
) : GcloudOptions;