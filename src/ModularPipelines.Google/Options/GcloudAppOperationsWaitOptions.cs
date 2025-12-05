using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "operations", "wait")]
public record GcloudAppOperationsWaitOptions(
[property: CliArgument] string Operation
) : GcloudOptions;