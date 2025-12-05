using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "describe")]
public record GcloudDataprocWorkflowTemplatesDescribeOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--version")]
    public new string? Version { get; set; }
}