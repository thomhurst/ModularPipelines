using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "export")]
public record GcloudDataprocWorkflowTemplatesExportOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}