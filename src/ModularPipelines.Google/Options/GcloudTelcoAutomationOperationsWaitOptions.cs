using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("telco-automation", "operations", "wait")]
public record GcloudTelcoAutomationOperationsWaitOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;