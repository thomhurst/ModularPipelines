using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "script-action", "show-execution-details")]
public record AzHdinsightScriptActionShowExecutionDetailsOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--execution-id")] string ExecutionId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;