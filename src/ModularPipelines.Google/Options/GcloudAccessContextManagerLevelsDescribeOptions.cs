using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "levels", "describe")]
public record GcloudAccessContextManagerLevelsDescribeOptions(
[property: CliArgument] string Level,
[property: CliArgument] string Policy
) : GcloudOptions;