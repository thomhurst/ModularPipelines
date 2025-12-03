using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "set-cluster-selector")]
public record GcloudDataprocWorkflowTemplatesSetClusterSelectorOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--cluster-labels")]
    public IEnumerable<KeyValue>? ClusterLabels { get; set; }
}