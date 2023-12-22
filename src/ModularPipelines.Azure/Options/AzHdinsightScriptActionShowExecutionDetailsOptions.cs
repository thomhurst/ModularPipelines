using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "script-action", "show-execution-details")]
public record AzHdinsightScriptActionShowExecutionDetailsOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--execution-id")] string ExecutionId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;