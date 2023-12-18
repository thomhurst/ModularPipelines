using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark-job-definition", "list")]
public record AzSynapseSparkJobDefinitionListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;