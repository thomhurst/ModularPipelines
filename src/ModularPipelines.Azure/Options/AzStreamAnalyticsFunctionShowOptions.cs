using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics", "function", "show")]
public record AzStreamAnalyticsFunctionShowOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;