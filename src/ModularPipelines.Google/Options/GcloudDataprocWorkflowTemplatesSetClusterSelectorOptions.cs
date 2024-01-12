using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "set-cluster-selector")]
public record GcloudDataprocWorkflowTemplatesSetClusterSelectorOptions(
[property: PositionalArgument] string Template,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--cluster-labels")]
    public IEnumerable<KeyValue>? ClusterLabels { get; set; }
}