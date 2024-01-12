using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "export")]
public record GcloudDataprocWorkflowTemplatesExportOptions(
[property: PositionalArgument] string Template,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}