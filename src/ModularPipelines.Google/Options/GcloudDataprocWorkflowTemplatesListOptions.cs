using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "list")]
public record GcloudDataprocWorkflowTemplatesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}