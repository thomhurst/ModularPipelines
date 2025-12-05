using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "targets", "delete")]
public record GcloudDeployTargetsDeleteOptions(
[property: CliArgument] string Target,
[property: CliArgument] string Region
) : GcloudOptions;