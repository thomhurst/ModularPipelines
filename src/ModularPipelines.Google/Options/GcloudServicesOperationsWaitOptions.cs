using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "operations", "wait")]
public record GcloudServicesOperationsWaitOptions(
[property: CliArgument] string Operation
) : GcloudOptions;