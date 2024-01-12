using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "list")]
public record GcloudDataprocWorkflowTemplatesListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}