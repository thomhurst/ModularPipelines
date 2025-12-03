using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "operations", "wait")]
public record GcloudEndpointsOperationsWaitOptions(
[property: CliArgument] string Operation
) : GcloudOptions;