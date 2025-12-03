using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "script-action", "promote")]
public record AzHdinsightScriptActionPromoteOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--execution-id")] string ExecutionId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;