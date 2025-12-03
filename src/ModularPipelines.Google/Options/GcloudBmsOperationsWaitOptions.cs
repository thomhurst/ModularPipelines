using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "operations", "wait")]
public record GcloudBmsOperationsWaitOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;