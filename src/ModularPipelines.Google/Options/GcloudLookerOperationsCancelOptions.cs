using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "operations", "cancel")]
public record GcloudLookerOperationsCancelOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;