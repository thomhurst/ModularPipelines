using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "script-action", "promote")]
public record AzHdinsightScriptActionPromoteOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--execution-id")] string ExecutionId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;