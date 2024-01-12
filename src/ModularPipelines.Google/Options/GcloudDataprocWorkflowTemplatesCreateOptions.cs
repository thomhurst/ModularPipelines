using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "create")]
public record GcloudDataprocWorkflowTemplatesCreateOptions(
[property: PositionalArgument] string Template,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--dag-timeout")]
    public string? DagTimeout { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}