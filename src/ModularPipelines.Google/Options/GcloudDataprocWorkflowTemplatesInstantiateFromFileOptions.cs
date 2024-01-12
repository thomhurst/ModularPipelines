using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "instantiate-from-file")]
public record GcloudDataprocWorkflowTemplatesInstantiateFromFileOptions(
[property: CommandSwitch("--file")] string File
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}